using Inventory.Application.Interface;
using Inventory.Domain.Model;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class TaxRepository : ITaxRepository
    {
        private readonly AppDbContext _context;

        public TaxRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tax>> GetAllActiveTaxesAsync()
        {
            return await _context.Taxes
                .AsNoTracking()
                .Where(t => t.IsActive)
                .OrderBy(t => t.Percentage)
                .ToListAsync();
        }
    }
}
