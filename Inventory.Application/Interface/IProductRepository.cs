using Inventory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Interface
{
    public interface IProductRepository
    {
        Task<Product?> GetProductWithTaxesAsync(int id);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product> AddProductWithStockAsync(Product product, InventoryStock initialStock);
        Task UpdateAsync(Product product);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<bool> SkuExistsAsync(string sku, int? excludeId = null);
        Task<bool> ClassificationExistsAsync(int classificationId);
        Task<List<int>> GetValidTaxIdsAsync(IEnumerable<int> taxIds);
    }
}
