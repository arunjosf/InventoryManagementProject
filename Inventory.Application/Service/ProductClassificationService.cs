using Inventory.Application.DTOS;
using Inventory.Application.Interface;
using Inventory.Domain.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Application.Service
{
    public interface IProductClassificationService
    {
        Task<ApiResponse<int>> CreateAsync(CreateProductClassificationDto dto);
        Task<ApiResponse<object>> GetAllAsync();
        Task<ApiResponse<object>> GetDropdownAsync();
    }

    public class ProductClassificationService : IProductClassificationService
    {
        private readonly IProductClassificationRepository _repo;

        public ProductClassificationService(IProductClassificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateProductClassificationDto dto)
        {
            var classification = new ProductClassification
            {
                Name = dto.Name,
                Code = dto.Code,
                ParentClassificationId = dto.ParentClassificationId
            };
            await _repo.AddAsync(classification);
            return new ApiResponse<int>(true, "Created", classification.Id);
        }

        public async Task<ApiResponse<object>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            var dtos = items.Select(i => new { i.Id, i.Name, i.Code, i.ParentClassificationId });
            return new ApiResponse<object>(true, "Success", dtos);
        }

        public async Task<ApiResponse<object>> GetDropdownAsync()
        {
            var items = (await _repo.GetAllAsync()).ToList();
            
            var dtos = items.Select(i => new 
            {
                Id = i.Id,
                Label = BuildCategoryPath(i, items)
            }).OrderBy(x => x.Label).ToList();

            return new ApiResponse<object>(true, "Success", dtos);
        }

        private string BuildCategoryPath(ProductClassification item, System.Collections.Generic.List<ProductClassification> allItems)
        {
            if (item.ParentClassificationId == null)
                return item.Name;
            
            var parent = allItems.FirstOrDefault(p => p.Id == item.ParentClassificationId);
            if (parent == null)
                return item.Name;
                
            return $"{BuildCategoryPath(parent, allItems)} > {item.Name}";
        }
    }
}
