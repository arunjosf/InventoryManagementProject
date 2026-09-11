using Inventory.Application.Interface;
using Inventory.Domain.Model;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task<(System.Collections.Generic.IEnumerable<Supplier> Suppliers, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Suppliers.AsNoTracking();
            var totalCount = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(query);
            var suppliers = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                System.Linq.Queryable.Take(System.Linq.Queryable.Skip(System.Linq.Queryable.OrderBy(query, s => s.Id), (pageNumber - 1) * pageSize), pageSize)
            );
            return (suppliers, totalCount);
        }
    }
}
