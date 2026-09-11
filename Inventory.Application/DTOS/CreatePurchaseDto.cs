using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOS
{
    public class CreatePurchaseDto
    {
        [Required(ErrorMessage = "Supplier ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Supplier ID must be a valid positive integer")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Invoice number is required")]
        [MaxLength(100, ErrorMessage = "Invoice number cannot exceed 100 characters")]
        public string InvoiceNumber { get; set; } = null!;

        [Required(ErrorMessage = "Invoice date is required")]
        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "Purchase items are required")]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<CreatePurchaseItemDto> Items { get; set; } = new();
    }
}
