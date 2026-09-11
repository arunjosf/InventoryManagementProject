using Inventory.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Application.Interface
{
    public interface ITaxRepository
    {
        Task<IEnumerable<Tax>> GetAllActiveTaxesAsync();
    }
}
