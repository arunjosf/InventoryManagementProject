using System;

namespace Inventory.Application.DTOS
{
    public class CreatePurchaseItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
