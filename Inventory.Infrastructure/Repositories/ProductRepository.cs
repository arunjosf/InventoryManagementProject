using Inventory.Application.Interface;
using Inventory.Domain.Model;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductWithTaxesAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductTaxes).ThenInclude(pt => pt.Tax)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> AddProductWithStockAsync(Product product, InventoryStock initialStock)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                initialStock.ProductId = product.Id;
                _context.InventoryStocks.Add(initialStock);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return product;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<(System.Collections.Generic.IEnumerable<Product> Products, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Products.AsNoTracking();
            var totalCount = await query.CountAsync();
            var products = await query
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (products, totalCount);
        }
    }
}
