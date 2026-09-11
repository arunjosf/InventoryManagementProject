using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOS
{
    public class CreatePurchaseItemDto
    {
        [Required(ErrorMessage = "Product ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be a valid positive integer")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
        public decimal UnitPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative")]
        public decimal DiscountAmount { get; set; }

        [Required(ErrorMessage = "Lot number is required")]
        [MaxLength(50, ErrorMessage = "Lot number cannot exceed 50 characters")]
        public string LotNumber { get; set; } = null!;

        [Required(ErrorMessage = "Expiry date is required")]
        public DateTime ExpiryDate { get; set; }
    }
}
