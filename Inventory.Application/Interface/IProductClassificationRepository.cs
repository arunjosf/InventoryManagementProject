using Inventory.Application.DTOS;
using Inventory.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Application.Interface
{
    public interface IProductClassificationRepository
    {
        Task<ProductClassification> AddAsync(ProductClassification classification);
        Task<IEnumerable<ProductClassification>> GetAllAsync();
    }
}
