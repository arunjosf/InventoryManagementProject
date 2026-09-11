using Inventory.Application.DTOS;
using System.Threading.Tasks;

namespace Inventory.Application.Interface
{
    public interface IInventoryService
    {
        Task<ApiResponse<AvailableStockResponseDto>> GetAvailableStockAsync(int productId);
    }
}
