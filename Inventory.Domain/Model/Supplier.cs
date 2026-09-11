using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Model
{
    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
            = new List<Purchase>();
    }
}
