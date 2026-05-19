using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneStopMobileRepair.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OneStopMobileRepair.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _env = env;
        }

        // LOGIN PAGE
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN POST
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid Email or Password";
            return View();
        }

        // LOGOUT
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "Website");
        }

        // ACCESS DENIED
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            TempData["Error"] = "Access Denied. Please log in again.";
            return RedirectToAction("Login", "Account");
        }

        // PROFILE PAGE
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) 
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }

            var model = new AdminProfileViewModel
            {
                FullName = user.FullName,
                ExistingProfilePictureUrl = user.ProfilePictureUrl
            };

            return View(model);
        }

        // PROFILE POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(AdminProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "User session expired. Please log in again.";
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                model.ExistingProfilePictureUrl = user.ProfilePictureUrl;
                return View(model);
            }

            bool profileUpdated = false;

            // Handle Profile Picture Removal
            if (model.RemovePicture)
            {
                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    string oldFilePath = Path.Combine(_env.WebRootPath, user.ProfilePictureUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    user.ProfilePictureUrl = null;
                    profileUpdated = true;
                }
            }
            // Handle Profile Picture Upload
            else if (model.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "profiles");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(fileStream);
                }

                user.ProfilePictureUrl = "/images/profiles/" + uniqueFileName;
                profileUpdated = true;
            }

            if (user.FullName != model.FullName)
            {
                user.FullName = model.FullName;
                profileUpdated = true;
            }

            if (profileUpdated)
            {
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    model.ExistingProfilePictureUrl = user.ProfilePictureUrl;
                    return View(model);
                }
                await _signInManager.RefreshSignInAsync(user);
            }

            // Handle Password Change
            if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    model.ExistingProfilePictureUrl = user.ProfilePictureUrl;
                    return View(model);
                }
                
                // Refresh sign-in cookie
                await _signInManager.RefreshSignInAsync(user);
            }

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }
    }
}