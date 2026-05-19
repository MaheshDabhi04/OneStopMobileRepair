using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OneStopMobileRepair.Models
{
    public class AdminProfileViewModel
    {
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        public string? ExistingProfilePictureUrl { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public bool RemovePicture { get; set; }
    }
}
