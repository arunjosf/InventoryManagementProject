using Inventory.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Application.Interface
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryStock>> GetAvailableFifoStockAsync(int productId);
        Task<IEnumerable<InventoryStock>> GetStockByProductIdAsync(int productId);
        Task AddStockAsync(InventoryStock stock);
    }
}
