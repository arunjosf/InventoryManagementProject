using Inventory.Application.DTOS;
using Inventory.Application.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Application.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepo;
        private readonly IProductRepository _productRepo;

        public InventoryService(IInventoryRepository inventoryRepo, IProductRepository productRepo)
        {
            _inventoryRepo = inventoryRepo;
            _productRepo = productRepo;
        }

        public async Task<ApiResponse<AvailableStockResponseDto>> GetAvailableStockAsync(int productId)
        {
            var product = await _productRepo.GetProductWithTaxesAsync(productId);
            if (product == null) 
                return new ApiResponse<AvailableStockResponseDto>(false, "Product not found", null);

            var stocks = await _inventoryRepo.GetAvailableFifoStockAsync(productId);

            var response = new AvailableStockResponseDto
            {
                Product = product.Name,
                TotalAvailableQuantity = stocks.Sum(s => s.AvailableQuantity),
                FifoStockQueue = stocks.Select(s => new FifoStockDto
                {
                    LotNumber = s.LotNumber,
                    ExpiryDate = s.ExpiryDate,
                    AvailableQuantity = s.AvailableQuantity
                }).ToList()
            };

            return new ApiResponse<AvailableStockResponseDto>(true, "Stock retrieved successfully", response);
        }
    }
}
