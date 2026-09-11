using Inventory.Application.Interface;
using Inventory.Domain.Model;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class ProductClassificationRepository : IProductClassificationRepository
    {
        private readonly AppDbContext _context;

        public ProductClassificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductClassification> AddAsync(ProductClassification classification)
        {
            _context.ProductClassifications.Add(classification);
            await _context.SaveChangesAsync();
            return classification;
        }

        public async Task<IEnumerable<ProductClassification>> GetAllAsync()
        {
            return await _context.ProductClassifications.AsNoTracking().ToListAsync();
        }
    }
}
