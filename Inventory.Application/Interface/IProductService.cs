using Inventory.Application.DTOS;
using System.Threading.Tasks;

namespace Inventory.Application.Interface
{
    public interface IProductService
    {
        Task<ApiResponse<ProductDto>> GetByIdAsync(int id);
        Task<PagedResponseDto<ProductDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<ApiResponse<int>> CreateAsync(CreateProductDto dto);
        Task<ApiResponse> UpdateAsync(int id, UpdateProductDto dto);
        Task<ApiResponse> DeleteAsync(int id);
    }
}
