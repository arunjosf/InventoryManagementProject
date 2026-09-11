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
                .Where(x => x.ProductId == productId && x.AvailableQuantity > 0 && (!x.ExpiryDate.HasValue || x.ExpiryDate.Value.Date > currentDate))
                .OrderBy(x => x.ExpiryDate ?? DateTime.MaxValue)
                .ThenBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryStock>> GetStockByProductIdAsync(int productId)
        {
            return await _context.InventoryStocks
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }

        public async Task AddStockAsync(InventoryStock stock)
        {
            _context.InventoryStocks.Add(stock);
            await _context.SaveChangesAsync();
        }
    }
}
