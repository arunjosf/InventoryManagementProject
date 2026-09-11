using Inventory.Domain.Model;
using System.Threading.Tasks;

namespace Inventory.Application.Interface
{
    public interface ISupplierRepository
    {
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier> AddAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task<(System.Collections.Generic.IEnumerable<Supplier> Suppliers, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
    }
}
