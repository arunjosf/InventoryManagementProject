using System;

namespace Inventory.Domain.Model
{
    public class ProductTax
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int TaxId { get; set; }
        public Tax Tax { get; set; } = null!;
    }
}
