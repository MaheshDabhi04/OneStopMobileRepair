using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneStopMobileRepair.Models;
using System.Linq;


namespace OneStopMobileRepair.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index(string status, string filter)
        {
            var customers = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                customers = customers.Where(x => x.Status == status);
                ViewData["CurrentFilter"] = "Status: " + status;
            }

            if (filter == "today")
            {
                var today = DateTime.Today;
                customers = customers.Where(x => x.CreditTime == today && x.Status != "Delivered" && x.Status != "Completed");
                ViewData["CurrentFilter"] = "Delivery Today";
            }
            else if (filter == "overdue")
            {
                var today = DateTime.Today;
                customers = customers.Where(x => x.CreditTime < today && x.Status != "Delivered" && x.Status != "Completed");
                ViewData["CurrentFilter"] = "Overdue Jobs";
            }

            return View(customers.ToList());
        }

        // CREATE PAGE
        public IActionResult Create()
        {
            return View();
        }

        // SAVE CUSTOMER WITH IMAGE
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // EDIT PAGE
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // LIVE SEARCH
        public IActionResult Search(string term)
        {
            var customers = _context.Customers
                .Where(c => c.CustomerName.Contains(term) || c.MobileNumber.Contains(term))
                .ToList();

            return PartialView("_CustomerTable", customers);
        }

        public IActionResult Reminder()
        {
            var today = DateTime.Today;

            var reminders = _context.Customers
                .Where(x => x.CreditTime <= today && x.Status != "Delivered" && x.Status != "Completed")
                .ToList();

            return View(reminders);
        }


        [AllowAnonymous]
        public IActionResult SeedDummyData()
        {
            var today = DateTime.Today;

            var dummyCustomers = new List<Customer>
            {
                new Customer { CustomerName = "John Doe", MobileNumber = "9876543210", Brand = "Samsung", Model = "Galaxy S21", Status = "Pending", CreditTime = today.AddDays(-2), Problem = "Screen Replacement", Estimate = 2500, Deposit = 500, EntryDate = today.AddDays(-5) },
                new Customer { CustomerName = "Alice Smith", MobileNumber = "9123456780", Brand = "Apple", Model = "iPhone 13", Status = "UnRepairable", CreditTime = today.AddDays(-1), Problem = "Battery Issue", Estimate = 3000, Deposit = 0, EntryDate = today.AddDays(-4) },
                new Customer { CustomerName = "Bob Johnson", MobileNumber = "9988776655", Brand = "OnePlus", Model = "9 Pro", Status = "Completed", CreditTime = today, Problem = "Charging Port", Estimate = 1500, Deposit = 1500, EntryDate = today.AddDays(-2) },
                new Customer { CustomerName = "Emma Brown", MobileNumber = "9871234567", Brand = "Xiaomi", Model = "Redmi Note 10", Status = "Pending", CreditTime = today.AddDays(1), Problem = "Water Damage", Estimate = 1800, Deposit = 500, EntryDate = today },
                new Customer { CustomerName = "Michael Davis", MobileNumber = "9654321098", Brand = "Vivo", Model = "V21", Status = "Delivered", CreditTime = today.AddDays(-3), Problem = "Speaker not working", Estimate = 800, Deposit = 800, EntryDate = today.AddDays(-6) }
            };

            _context.Customers.AddRange(dummyCustomers);
            _context.SaveChanges();

            return Content("Dummy data added successfully! 5 records inserted for Reminder Testing.");
        }
    }
}