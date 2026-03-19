using Microsoft.AspNetCore.Mvc;
using OneStopMobileRepair.Models;
using System.Linq;

namespace OneStopMobileRepair.Controllers
{
    public class EnquiriesController : Controller
    {
        private readonly AppDbContext _context;

        public EnquiriesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // PROACTIVE: Create table if it doesn't exist (Fixing Error 208)
            string createTableSql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Enquiries' AND xtype='U')
                CREATE TABLE Enquiries (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(MAX) NOT NULL,
                    Phone NVARCHAR(MAX) NOT NULL,
                    Service NVARCHAR(MAX),
                    Device NVARCHAR(MAX),
                    Message NVARCHAR(MAX),
                    CreatedAt DATETIME NOT NULL,
                    Status NVARCHAR(MAX),
                    AdminReply NVARCHAR(MAX),
                    ReplyDate DATETIME
                )
                ELSE
                BEGIN
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Enquiries') AND name = 'AdminReply')
                        ALTER TABLE Enquiries ADD AdminReply NVARCHAR(MAX);
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Enquiries') AND name = 'ReplyDate')
                        ALTER TABLE Enquiries ADD ReplyDate DATETIME;
                END";
            
            try 
            {
                Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.ExecuteSqlRaw(_context.Database, createTableSql);
            }
            catch { /* Ignore if already exists or other sql issues */ }

            var enquiries = _context.Enquiries.OrderByDescending(x => x.CreatedAt).ToList();
            return View(enquiries);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var enquiry = _context.Enquiries.Find(id);
            if (enquiry != null)
            {
                enquiry.Status = status;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var enquiry = _context.Enquiries.Find(id);
            if (enquiry != null)
            {
                _context.Enquiries.Remove(enquiry);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult AddReply(int id, string reply)
        {
            var enquiry = _context.Enquiries.Find(id);
            if (enquiry != null)
            {
                enquiry.AdminReply = reply;
                enquiry.ReplyDate = DateTime.Now;
                enquiry.Status = "Resolved"; // Automatically mark as resolved when replied
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
