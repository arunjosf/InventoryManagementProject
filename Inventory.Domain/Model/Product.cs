using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Model
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string SKU { get; set; } = null!;

        public int ProductClassificationId { get; set; }
        public ProductClassification ProductClassification { get; set; } = null!;

        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }

        public bool IsActive { get; set; }

        public ICollection<ProductTax> ProductTaxes { get; set; } = new List<ProductTax>();

        public ICollection<PurchaseItem> PurchaseItems { get; set; }
            = new List<PurchaseItem>();
    }
}
    