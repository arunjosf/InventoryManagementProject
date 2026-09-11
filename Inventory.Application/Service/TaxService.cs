using Inventory.Application.DTOS;
using Inventory.Application.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Application.Service
{
    public interface ITaxService
    {
        Task<ApiResponse<IEnumerable<TaxDto>>> GetActiveTaxesAsync();
    }

    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepo;

        public TaxService(ITaxRepository taxRepo)
        {
            _taxRepo = taxRepo;
        }

        public async Task<ApiResponse<IEnumerable<TaxDto>>> GetActiveTaxesAsync()
        {
            var taxes = await _taxRepo.GetAllActiveTaxesAsync();
            var dtos = taxes.Select(t => new TaxDto
            {
                Id = t.Id,
                Name = t.Name,
                Percentage = t.Percentage
            });

            return new ApiResponse<IEnumerable<TaxDto>>(true, "Active taxes retrieved successfully.", dtos);
        }
    }
}
