using System;

namespace Inventory.Domain.Model
{
    public class PurchaseItemTax
    {
        public int Id { get; set; }

        public int PurchaseItemId { get; set; }
        public PurchaseItem PurchaseItem { get; set; } = null!;

        public string TaxName { get; set; } = null!;
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
    }
}
