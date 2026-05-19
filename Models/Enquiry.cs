using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace OneStopMobileRepair.Models
{
    public class Enquiry
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must contain letters only.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be exactly 10 digits.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a service.")]
        public string Service { get; set; } = string.Empty;

        [Required(ErrorMessage = "Device model is required.")]
        [StringLength(200)]
        public string Device { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Status { get; set; } = "New"; // New, Contacted, Resolved, Junk

        public string? AdminReply { get; set; }
        public DateTime? ReplyDate { get; set; }

        public string? FrontImagePath { get; set; }
        public string? BackImagePath { get; set; }

        [NotMapped]
        public IFormFile? FrontImageFile { get; set; }

        [NotMapped]
        public IFormFile? BackImageFile { get; set; }
    }
}

