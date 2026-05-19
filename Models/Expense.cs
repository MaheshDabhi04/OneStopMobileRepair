using System;
using System.ComponentModel.DataAnnotations;

namespace OneStopMobileRepair.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name/Description is required.")]
        [StringLength(200, ErrorMessage = "Product Name cannot exceed 200 characters.")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Expense Date is required.")]
        public DateTime ExpenseDate { get; set; } = DateTime.Now;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
