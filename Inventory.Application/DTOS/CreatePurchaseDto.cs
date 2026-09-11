using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOS
{
    public class CreatePurchaseDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Supplier ID is required")]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(100)]
        public string InvoiceNumber { get; set; } = null!;

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<CreatePurchaseItemDto> Items { get; set; } = new();
    }
}
