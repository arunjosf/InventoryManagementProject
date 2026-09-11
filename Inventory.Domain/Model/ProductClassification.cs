using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Model
{
    public class ProductClassification
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Code { get; set; }

        public int? ParentClassificationId { get; set; }
        public ProductClassification? ParentClassification { get; set; }

        public ICollection<ProductClassification> SubClassifications { get; set; } = new List<ProductClassification>();

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
