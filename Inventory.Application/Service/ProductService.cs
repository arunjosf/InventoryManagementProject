using Inventory.Application.DTOS;
using Inventory.Application.Interface;
using Inventory.Domain.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IInventoryRepository _inventoryRepo;

        public ProductService(IProductRepository productRepo, IInventoryRepository inventoryRepo)
        {
            _productRepo = productRepo;
            _inventoryRepo = inventoryRepo;
        }

        public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id)
        {
            var p = await _productRepo.GetByIdAsync(id);
            if (p == null) return new ApiResponse<ProductDto>(false, "Not found", null);

            var stocks = (await _inventoryRepo.GetStockByProductIdAsync(id)).ToList();
            var isExpired = stocks.Any() && stocks.Where(x => x.AvailableQuantity > 0).All(x => x.ExpiryDate.HasValue && x.ExpiryDate.Value.Date < DateTime.UtcNow.Date);

            var dto = new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                IsActive = p.IsActive,
                TotalAvailableStock = stocks.Sum(x => x.AvailableQuantity),
                HasExpiredStock = isExpired
            };
            return new ApiResponse<ProductDto>(true, "Success", dto);
        }

        public async Task<PagedResponseDto<ProductDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var (products, total) = await _productRepo.GetAllAsync(pageNumber, pageSize);
            
            var dtos = new System.Collections.Generic.List<ProductDto>();
            foreach (var p in products)
            {
                var stocks = (await _inventoryRepo.GetStockByProductIdAsync(p.Id)).ToList();
                var isExpired = stocks.Any() && stocks.Where(x => x.AvailableQuantity > 0).All(x => x.ExpiryDate.HasValue && x.ExpiryDate.Value.Date < DateTime.UtcNow.Date);
                
                dtos.Add(new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    PurchasePrice = p.PurchasePrice,
                    SellingPrice = p.SellingPrice,
                    IsActive = p.IsActive,
                    TotalAvailableStock = stocks.Sum(x => x.AvailableQuantity),
                    HasExpiredStock = isExpired
                });
            }
            
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
                IsActive = true
            };

            var initialStock = new InventoryStock
            {
                LotNumber = dto.InitialLotNumber,
                ExpiryDate = dto.InitialExpiryDate,
                AvailableQuantity = dto.InitialStockQuantity
            };
            
            await _productRepo.AddProductWithStockAsync(p, initialStock);
            return new ApiResponse<int>(true, "Created successfully", p.Id);
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateProductDto dto)
        {
            var p = await _productRepo.GetByIdAsync(id);
            if (p == null) return new ApiResponse(false, "Not found");

            p.Name = dto.Name;
            p.SKU = dto.SKU;
            p.PurchasePrice = dto.PurchasePrice;
            p.SellingPrice = dto.SellingPrice;

            await _productRepo.UpdateAsync(p);
            return new ApiResponse(true, "Updated successfully");
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
