using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace OneStopMobileRepair.Models
{
    public class Enquiry
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? Service { get; set; }

        public string? Device { get; set; }

        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? Status { get; set; } = "New"; // New, Contacted, Resolved, Junk

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
