using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Interface;
using Inventory.Application.DTOS;
using System.Threading.Tasks;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchasesController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchase([FromBody] CreatePurchaseDto request)
        {
            var result = await _purchaseService.CreatePurchaseAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("{id}/post")]
        public async Task<IActionResult> PostPurchase(int id)
        {
            var result = await _purchaseService.PostPurchaseAsync(id);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPurchases([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _purchaseService.GetAllPurchasesAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}
