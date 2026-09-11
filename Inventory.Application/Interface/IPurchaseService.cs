using Inventory.Application.DTOS;
using System.Threading.Tasks;

namespace Inventory.Application.Interface
{
    public interface IPurchaseService
    {
        Task<ApiResponse<int>> CreatePurchaseAsync(CreatePurchaseDto request);
        Task<ApiResponse> PostPurchaseAsync(int purchaseId);
        Task<PagedResponseDto<PurchaseDto>> GetAllPurchasesAsync(int pageNumber, int pageSize);
    }
}
