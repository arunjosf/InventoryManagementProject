using Inventory.Application.Interface;
using Inventory.Domain.Model;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _context;

        public PurchaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Purchase> CreatePurchaseAsync(Purchase purchase)
        {
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return purchase;
        }

        public async Task<Purchase?> GetPurchaseWithItemsAsync(int id)
        {
            return await _context.Purchases
                .Include(p => p.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> PostPurchaseAsync(int purchaseId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var purchase = await GetPurchaseWithItemsAsync(purchaseId);
                if (purchase == null || purchase.IsPosted) return false;

                foreach (var item in purchase.Items)
                {
                    var stock = await _context.InventoryStocks.FirstOrDefaultAsync(s => 
                        s.ProductId == item.ProductId && 
                        s.LotNumber == item.LotNumber);

                    if (stock != null)
                    {
                        stock.AvailableQuantity += item.Quantity;
                    }
                    else
                    {
                        _context.InventoryStocks.Add(new InventoryStock
                        {
                            ProductId = item.ProductId,
                            LotNumber = item.LotNumber,
                            ExpiryDate = item.ExpiryDate,
                            AvailableQuantity = item.Quantity
                        });
                    }
                }

                purchase.IsPosted = true;
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<(System.Collections.Generic.IEnumerable<Purchase> Purchases, int TotalCount)> GetPurchasesAsync(int pageNumber, int pageSize)
        {
            var query = _context.Purchases.AsNoTracking();
            var totalCount = await query.CountAsync();
            
            var purchases = await query
                .OrderByDescending(p => p.PurchaseDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (purchases, totalCount);
        }
    }
}
