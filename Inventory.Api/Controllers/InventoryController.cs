using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Interface;
using Inventory.Application.DTOS;
using System.Threading.Tasks;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("{productId}/available-stock")]
        public async Task<ActionResult<ApiResponse<AvailableStockResponseDto>>> GetAvailableStock(int productId)
        {
            var result = await _inventoryService.GetAvailableStockAsync(productId);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
