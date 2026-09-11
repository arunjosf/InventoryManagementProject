using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOS
{
    public class CreatePurchaseItemDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
        public decimal UnitPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative")]
        public decimal DiscountAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string LotNumber { get; set; } = null!;

        [Required]
        public DateTime ExpiryDate { get; set; }
    }
}
