using Inventory.Application.DTOS;
using Inventory.Application.Interface;
using Inventory.Domain.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id)
        {
            var p = await _productRepo.GetByIdAsync(id);
            if (p == null) return new ApiResponse<ProductDto>(false, "Not found", null);

            var dto = new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                IsActive = p.IsActive,
                IsLotTrackingEnabled = p.IsLotTrackingEnabled,
                IsExpiryTrackingEnabled = p.IsExpiryTrackingEnabled
            };
            return new ApiResponse<ProductDto>(true, "Success", dto);
        }

        public async Task<PagedResponseDto<ProductDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var (products, total) = await _productRepo.GetAllAsync(pageNumber, pageSize);
            var dtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                IsActive = p.IsActive,
                IsLotTrackingEnabled = p.IsLotTrackingEnabled,
                IsExpiryTrackingEnabled = p.IsExpiryTrackingEnabled
            });
            return new PagedResponseDto<ProductDto>(dtos, pageNumber, pageSize, total);
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateProductDto dto)
        {
            var p = new Product
            {
                Name = dto.Name,
                SKU = dto.SKU,
                ProductClassificationId = dto.ProductClassificationId,
                PurchasePrice = dto.PurchasePrice,
                SellingPrice = dto.SellingPrice,
                IsLotTrackingEnabled = dto.IsLotTrackingEnabled,
                IsExpiryTrackingEnabled = dto.IsExpiryTrackingEnabled,
                IsActive = true
            };
            await _productRepo.AddAsync(p);
            return new ApiResponse<int>(true, "Created", p.Id);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var p = await _productRepo.GetByIdAsync(id);
            if (p == null) return new ApiResponse(false, "Not found");

            p.IsActive = false;
            await _productRepo.UpdateAsync(p);
            return new ApiResponse(true, "Deleted (Soft)");
        }
    }
}
