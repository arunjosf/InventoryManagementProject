using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Model
{
    public class PurchaseItem
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal DiscountAmount { get; set; }
        public string? LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<PurchaseItemTax> Taxes { get; set; } = new List<PurchaseItemTax>();
    }
}
