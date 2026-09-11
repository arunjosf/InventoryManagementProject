using Inventory.Domain.Model;
using System.Threading.Tasks;

namespace Inventory.Application.Interface
{
    public interface IPurchaseRepository
    {
        Task<Purchase> CreatePurchaseAsync(Purchase purchase);
        Task<Purchase?> GetPurchaseWithItemsAsync(int id);
        Task<bool> PostPurchaseAsync(int purchaseId);
        Task<(System.Collections.Generic.IEnumerable<Purchase> Purchases, int TotalCount)> GetPurchasesAsync(int pageNumber, int pageSize);
    }
}
