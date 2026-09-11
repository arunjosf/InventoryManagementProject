using Inventory.Application.DTOS;
using Inventory.Application.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductClassificationsController : ControllerBase
    {
        private readonly IProductClassificationService _service;

        public ProductClassificationsController(IProductClassificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductClassificationDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }
    }
}
