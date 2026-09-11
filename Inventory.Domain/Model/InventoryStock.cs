using System;

namespace Inventory.Domain.Model
{
    public class InventoryStock
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string? LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public int AvailableQuantity { get; set; }
    }
}
