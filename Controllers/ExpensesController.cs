using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneStopMobileRepair.Models;
using System;
using System.Linq;

namespace OneStopMobileRepair.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly AppDbContext _context;

        public ExpensesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public IActionResult Index()
        {
            var expenses = _context.Expenses.OrderByDescending(e => e.ExpenseDate).ToList();

            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            ViewBag.DailyTotal = _context.Expenses
                .Where(e => e.ExpenseDate.Date == today)
                .Sum(e => (decimal?)e.Amount) ?? 0;

            ViewBag.MonthlyTotal = _context.Expenses
                .Where(e => e.ExpenseDate >= startOfMonth)
                .Sum(e => (decimal?)e.Amount) ?? 0;

            return View(expenses);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            return View(new Expense { ExpenseDate = DateTime.Now });
        }

        // POST: Expenses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Expenses.Add(expense);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = _context.Expenses
                .FirstOrDefault(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
