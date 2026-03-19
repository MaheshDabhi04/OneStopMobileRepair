using Microsoft.AspNetCore.Mvc;
using OneStopMobileRepair.Models;
using System.Threading.Tasks;

namespace OneStopMobileRepair.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly AppDbContext _context;

        public WebsiteController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(Enquiry enquiry)
        {
            Console.WriteLine($"[DEBUG] Received Enquiry from {enquiry.Name} ({enquiry.Phone})");
            
            if (ModelState.IsValid)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "enquiries");
                if (enquiry.FrontImageFile != null || enquiry.BackImageFile != null)
                {
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);
                }

                if (enquiry.FrontImageFile != null && enquiry.FrontImageFile.Length > 0)
                {
                    var frontFileName = Guid.NewGuid().ToString() + "_" + enquiry.FrontImageFile.FileName;
                    var frontFilePath = Path.Combine(uploadsFolder, frontFileName);
                    
                    using (var fileStream = new FileStream(frontFilePath, FileMode.Create))
                    {
                        await enquiry.FrontImageFile.CopyToAsync(fileStream);
                    }
                    
                    enquiry.FrontImagePath = "/uploads/enquiries/" + frontFileName;
                }

                if (enquiry.BackImageFile != null && enquiry.BackImageFile.Length > 0)
                {
                    var backFileName = Guid.NewGuid().ToString() + "_" + enquiry.BackImageFile.FileName;
                    var backFilePath = Path.Combine(uploadsFolder, backFileName);
                    
                    using (var fileStream = new FileStream(backFilePath, FileMode.Create))
                    {
                        await enquiry.BackImageFile.CopyToAsync(fileStream);
                    }
                    
                    enquiry.BackImagePath = "/uploads/enquiries/" + backFileName;
                }

                _context.Enquiries.Add(enquiry);
                await _context.SaveChangesAsync();
                Console.WriteLine("[DEBUG] Enquiry SAVED successfully.");
                return Json(new { success = true });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine("[DEBUG] Enquiry FAILED validation: " + string.Join(", ", errors));
            return Json(new { success = false, message = "Validation failed", errors = errors });
        }

        [HttpGet]
        public IActionResult CheckStatus()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckStatus(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                ViewBag.Error = "Please enter a valid mobile number.";
                return View();
            }

            var enquiries = _context.Enquiries
                                    .Where(e => e.Phone == phone)
                                    .OrderByDescending(e => e.CreatedAt)
                                    .ToList();

            ViewBag.Phone = phone;
            return View(enquiries);
        }
    }
}