using System;
using System.Collections.Generic;

namespace Inventory.Application.DTOS
{
    public class CreatePurchaseDto
    {
        public int SupplierId { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public List<CreatePurchaseItemDto> Items { get; set; } = new();
    }
}
