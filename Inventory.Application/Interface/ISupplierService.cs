using Inventory.Application.DTOS;
using System.Threading.Tasks;

namespace Inventory.Application.Interface
{
    public interface ISupplierService
    {
        Task<ApiResponse<SupplierDto>> GetByIdAsync(int id);
        Task<PagedResponseDto<SupplierDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<ApiResponse<int>> CreateAsync(CreateSupplierDto dto);
    }
}
