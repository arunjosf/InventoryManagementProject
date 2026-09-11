using System;
using System.Collections.Generic;

namespace Inventory.Domain.Model
{
    public class Tax
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Percentage { get; set; }
        public bool IsCompound { get; set; }
        public bool IsActive { get; set; }

        public ICollection<ProductTax> ProductTaxes { get; set; } = new List<ProductTax>();
    }
}
