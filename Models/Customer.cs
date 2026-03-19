using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneStopMobileRepair.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string? CustomerName { get; set; }

        public string? MobileNumber { get; set; }

        public string? Brand { get; set; }

        public string? Model { get; set; }

        public string? Problem { get; set; }

        public string DevicePassword { get; set; }

        public string? Status { get; set; }

        public int Estimate { get; set; }
        
        public int Deposit { get; set; }

        public DateTime? CreditTime { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.Now;
    }
}