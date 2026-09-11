using Inventory.Application.DTOS;
using Inventory.Application.Interface;
using Inventory.Domain.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Application.Service
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repo;

        public SupplierService(ISupplierRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<SupplierDto>> GetByIdAsync(int id)
        {
            var s = await _repo.GetByIdAsync(id);
            if (s == null) return new ApiResponse<SupplierDto>(false, "Not found", null);

            var dto = new SupplierDto { Id = s.Id, Name = s.Name, Phone = s.Phone, Email = s.Email };
            return new ApiResponse<SupplierDto>(true, "Success", dto);
        }

        public async Task<PagedResponseDto<SupplierDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var (suppliers, total) = await _repo.GetAllAsync(pageNumber, pageSize);
            var dtos = suppliers.Select(s => new SupplierDto { Id = s.Id, Name = s.Name, Phone = s.Phone, Email = s.Email });
            return new PagedResponseDto<SupplierDto>(dtos, pageNumber, pageSize, total);
        }

        public async Task<ApiResponse<int>> CreateAsync(CreateSupplierDto dto)
        {
            var s = new Supplier { Name = dto.Name, Phone = dto.Phone, Email = dto.Email };
            await _repo.AddAsync(s);
            return new ApiResponse<int>(true, "Created", s.Id);
        }
    }
}
