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
                // Ensure 0 is converted to null to avoid FK constraint errors
                ParentClassificationId = dto.ParentClassificationId > 0 ? dto.ParentClassificationId : null
            };
            await _repo.AddAsync(classification);
            return new ApiResponse<int>(true, "Created", classification.Id);
        }

        public async Task<ApiResponse<object>> GetAllAsync()
        {
            var items = (await _repo.GetAllAsync()).ToList();
            
            var dtos = items.Select(i => new 
            {
                Id = i.Id,
                Name = i.Name,
                Code = i.Code,
                ParentClassificationId = i.ParentClassificationId,
                DropdownLabel = BuildCategoryPath(i, items)
            }).OrderBy(x => x.DropdownLabel).ToList();

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
