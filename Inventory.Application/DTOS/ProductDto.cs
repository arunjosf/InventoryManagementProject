using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOS
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsActive { get; set; }
        
        // Stock summary properties
        public int TotalAvailableStock { get; set; }
        public bool HasExpiredStock { get; set; }
    }

    public class CreateProductDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string SKU { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Product classification is required")]
        public int ProductClassificationId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0")]
        public decimal PurchasePrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than 0")]
        public decimal SellingPrice { get; set; }

        // Initial stock requirement
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Initial stock cannot be negative")]
        public int InitialStockQuantity { get; set; }

        [Required]
        [MaxLength(50)]
        public string InitialLotNumber { get; set; } = null!;

        [Required]
        public DateTime InitialExpiryDate { get; set; }
    }

    public class UpdateProductDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string SKU { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0")]
        public decimal PurchasePrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than 0")]
        public decimal SellingPrice { get; set; }
    }
}
