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
            var p = await _productRepo.GetProductWithTaxesAsync(id);
            if (p == null) return new ApiResponse<ProductDto>(false, "Product not found.", null);

            var stocks = (await _inventoryRepo.GetStockByProductIdAsync(id)).ToList();
            var isExpired = stocks.Any() && stocks.Where(x => x.AvailableQuantity > 0).All(x => x.ExpiryDate.HasValue && x.ExpiryDate.Value.Date < DateTime.UtcNow.Date);

            var dto = new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                ProductClassificationId = p.ProductClassificationId,
                ClassificationName = p.ProductClassification?.Name ?? "General",
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                IsActive = p.IsActive,
                TotalAvailableStock = stocks.Sum(x => x.AvailableQuantity),
                HasExpiredStock = isExpired,
                TaxIds = p.ProductTaxes?.Select(pt => pt.TaxId).ToList() ?? new System.Collections.Generic.List<int>()
            };
            return new ApiResponse<ProductDto>(true, "Success", dto);
        }

        public async Task<PagedResponseDto<ProductDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var (products, total) = await _productRepo.GetAllAsync(pageNumber, pageSize);
            
            var dtos = new System.Collections.Generic.List<ProductDto>();
            foreach (var p in products)
            {
                var pWithTaxes = await _productRepo.GetProductWithTaxesAsync(p.Id);
                var stocks = (await _inventoryRepo.GetStockByProductIdAsync(p.Id)).ToList();
                var isExpired = stocks.Any() && stocks.Where(x => x.AvailableQuantity > 0).All(x => x.ExpiryDate.HasValue && x.ExpiryDate.Value.Date < DateTime.UtcNow.Date);
                
                dtos.Add(new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    ProductClassificationId = p.ProductClassificationId,
                    ClassificationName = p.ProductClassification?.Name ?? "General",
                    PurchasePrice = p.PurchasePrice,
                    SellingPrice = p.SellingPrice,
                    IsActive = p.IsActive,
                    TotalAvailableStock = stocks.Sum(x => x.AvailableQuantity),
                    HasExpiredStock = isExpired,
                    TaxIds = pWithTaxes?.ProductTaxes?.Select(pt => pt.TaxId).ToList() ?? new System.Collections.Generic.List<int>()
                });
            }
            
            return new PagedResponseDto<ProductDto>(dtos, pageNumber, pageSize, total);
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateProductDto dto)
        {
            // 1. Validate Product Classification
            var classificationExists = await _productRepo.ClassificationExistsAsync(dto.ProductClassificationId);
            if (!classificationExists)
            {
                return new ApiResponse<int>(false, $"Product classification ID {dto.ProductClassificationId} does not exist.", 0);
            }

            // 2. Validate SKU uniqueness
            var skuExists = await _productRepo.SkuExistsAsync(dto.SKU);
            if (skuExists)
            {
                return new ApiResponse<int>(false, $"A product with SKU '{dto.SKU}' already exists.", 0);
            }

            // 3. Validate Initial Stock and Lot
            if (string.IsNullOrWhiteSpace(dto.InitialLotNumber))
            {
                return new ApiResponse<int>(false, "Initial Lot Number is mandatory.", 0);
            }

            if (dto.InitialStockQuantity < 0)
            {
                return new ApiResponse<int>(false, "Initial Stock Quantity cannot be negative.", 0);
            }

            // Expiry Date (Date only required, no time needed)
            var expiryDateOnly = dto.InitialExpiryDate.Date;
            if (expiryDateOnly == default)
            {
                return new ApiResponse<int>(false, "Initial Expiry Date is required.", 0);
            }

            // 4. Validate and attach Taxes
            var validTaxIds = new System.Collections.Generic.List<int>();
            if (dto.TaxIds != null && dto.TaxIds.Any())
            {
                validTaxIds = await _productRepo.GetValidTaxIdsAsync(dto.TaxIds);
                if (validTaxIds.Count != dto.TaxIds.Distinct().Count())
                {
                    return new ApiResponse<int>(false, "One or more selected Tax IDs are invalid or inactive.", 0);
                }
            }

            var p = new Product
            {
                Name = dto.Name.Trim(),
                SKU = dto.SKU.Trim().ToUpper(),
                ProductClassificationId = dto.ProductClassificationId,
                PurchasePrice = dto.PurchasePrice,
                SellingPrice = dto.SellingPrice,
                IsActive = true,
                ProductTaxes = validTaxIds.Select(t => new ProductTax { TaxId = t }).ToList()
            };

            var initialStock = new InventoryStock
            {
                LotNumber = dto.InitialLotNumber.Trim(),
                ExpiryDate = expiryDateOnly,
                AvailableQuantity = dto.InitialStockQuantity
            };
            
            await _productRepo.AddProductWithStockAsync(p, initialStock);
            return new ApiResponse<int>(true, "Product created successfully with initial stock.", p.Id);
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateProductDto dto)
        {
            var p = await _productRepo.GetProductWithTaxesAsync(id);
            if (p == null) return new ApiResponse(false, "Product not found.");

            // Validate SKU uniqueness excluding current product
            var skuExists = await _productRepo.SkuExistsAsync(dto.SKU, id);
            if (skuExists)
            {
                return new ApiResponse(false, $"A product with SKU '{dto.SKU}' already exists.");
            }

            p.Name = dto.Name.Trim();
            p.SKU = dto.SKU.Trim().ToUpper();
            p.PurchasePrice = dto.PurchasePrice;
            p.SellingPrice = dto.SellingPrice;

            p.ProductTaxes.Clear();
            if (dto.TaxIds != null && dto.TaxIds.Any())
            {
                var validTaxIds = await _productRepo.GetValidTaxIdsAsync(dto.TaxIds);
                if (validTaxIds.Count != dto.TaxIds.Distinct().Count())
                {
                    return new ApiResponse(false, "One or more selected Tax IDs are invalid or inactive.");
                }

                foreach (var t in validTaxIds)
                {
                    p.ProductTaxes.Add(new ProductTax { ProductId = id, TaxId = t });
                }
            }

            await _productRepo.UpdateAsync(p);
            return new ApiResponse(true, "Product updated successfully.");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var p = await _productRepo.GetByIdAsync(id);
            if (p == null) return new ApiResponse(false, "Product not found.");

            p.IsActive = false;
            await _productRepo.UpdateAsync(p);
            return new ApiResponse(true, "Product deactivated successfully (Soft Deleted).");
        }
    }
}
