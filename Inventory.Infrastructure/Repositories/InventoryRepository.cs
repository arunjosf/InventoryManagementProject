using Inventory.Application.Interface;
using Inventory.Domain.Model;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryStock>> GetAvailableFifoStockAsync(int productId)
        {
            var currentDate = DateTime.UtcNow.Date;

            return await _context.InventoryStocks
                .Where(s => s.ProductId == productId && s.AvailableQuantity > 0 &&
                            (!s.ExpiryDate.HasValue || s.ExpiryDate.Value.Date > currentDate))
                .OrderBy(s => s.ExpiryDate ?? DateTime.MaxValue)
                .ThenBy(s => s.Id)
                .ToListAsync();
        }
    }
}
