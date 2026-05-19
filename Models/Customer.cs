using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneStopMobileRepair.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must contain letters only.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string? MobileNumber { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        public string? Brand { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        public string? Model { get; set; }

        [Required(ErrorMessage = "Problem Description is required.")]
        [StringLength(500, ErrorMessage = "Problem cannot exceed 500 characters.")]
        public string? Problem { get; set; }

        public string? DevicePassword { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }

        public int Estimate { get; set; }
        
        public int Deposit { get; set; }

        public DateTime? CreditTime { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.Now;
    }
}