using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneStopMobileRepair.Models;
using System.Linq;

namespace OneStopMobileRepair.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;

            ViewBag.Total = _context.Customers.Count();
            ViewBag.Pending = _context.Customers.Count(x => x.Status == "Pending");
            ViewBag.UnRepairable = _context.Customers.Count(x => x.Status == "Unrepairable");
            ViewBag.Completed = _context.Customers.Count(x => x.Status == "Completed");
            ViewBag.Delivered = _context.Customers.Count(x => x.Status == "Delivered");

            ViewBag.DeliveryToday = _context.Customers
                .Count(x => x.CreditTime == today && x.Status != "Delivered" && x.Status != "Completed");

            ViewBag.Overdue = _context.Customers
                .Count(x => x.CreditTime < today && x.Status != "Delivered" && x.Status != "Completed");

            // Expense Summary
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            ViewBag.DailyExpense = _context.Expenses
                .Where(e => e.ExpenseDate.Date == today)
                .Sum(e => (decimal?)e.Amount) ?? 0;

            ViewBag.MonthlyExpense = _context.Expenses
                .Where(e => e.ExpenseDate >= startOfMonth)
                .Sum(e => (decimal?)e.Amount) ?? 0;

            return View();
        }
    }
}