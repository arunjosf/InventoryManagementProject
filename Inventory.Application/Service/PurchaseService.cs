using Inventory.Application.DTOS;
using Inventory.Application.Interface;
using Inventory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Application.Service
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepo;
        private readonly IProductRepository _productRepo;
        private readonly ISupplierRepository _supplierRepo;

        public PurchaseService(
            IPurchaseRepository purchaseRepo,
            IProductRepository productRepo,
            ISupplierRepository supplierRepo)
        {
            _purchaseRepo = purchaseRepo;
            _productRepo = productRepo;
            _supplierRepo = supplierRepo;
        }

        public async Task<ApiResponse<int>> CreatePurchaseAsync(CreatePurchaseDto request)
        {
            if (request.Items == null || !request.Items.Any())
                return new ApiResponse<int>(false, "Purchase must have at least one item.", 0);

            // Validate that the same product is not purchased with the same lot multiple times in one invoice
            var duplicateLines = request.Items
                .GroupBy(i => new { i.ProductId, Lot = i.LotNumber?.Trim() })
                .Where(g => g.Count() > 1)
                .Any();

            if (duplicateLines)
                return new ApiResponse<int>(false, "Same product cannot be purchased multiple times with the same lot in a single transaction.", 0);

            var supplier = await _supplierRepo.GetByIdAsync(request.SupplierId);
            if (supplier == null)
                return new ApiResponse<int>(false, "Supplier not found.", 0);

            var purchase = new Purchase
            {
                SupplierId = request.SupplierId,
                InvoiceNumber = request.InvoiceNumber,
                InvoiceDate = request.InvoiceDate,
                PurchaseDate = DateTime.UtcNow,
                IsPosted = false, // starts as draft
                SubTotal = 0,
                TaxAmount = 0,
                GrandTotal = 0,
                Items = new List<PurchaseItem>()
            };

            foreach (var itemReq in request.Items)
            {
                if (itemReq.Quantity <= 0)
                    return new ApiResponse<int>(false, $"Quantity must be greater than zero for product ID {itemReq.ProductId}.", 0);

                if (itemReq.UnitPrice < 0)
                    return new ApiResponse<int>(false, $"Unit price cannot be negative for product ID {itemReq.ProductId}.", 0);

                if (itemReq.DiscountAmount < 0 || itemReq.DiscountAmount > (itemReq.Quantity * itemReq.UnitPrice))
                    return new ApiResponse<int>(false, $"Invalid discount amount for product ID {itemReq.ProductId}.", 0);

                var product = await _productRepo.GetProductWithTaxesAsync(itemReq.ProductId);

                if (product == null)
                    return new ApiResponse<int>(false, $"Product with ID {itemReq.ProductId} not found.", 0);
                
                if (!product.IsActive)
                    return new ApiResponse<int>(false, $"Product {product.Name} is inactive and cannot be purchased.", 0);

                if (string.IsNullOrWhiteSpace(itemReq.LotNumber))
                    return new ApiResponse<int>(false, $"Lot Number is mandatory for product {product.Name}.", 0);

                var compareDate = request.InvoiceDate;
                
                // Expiry date is now mandatory in DTO, but ensure it's not default/invalid
                if (itemReq.ExpiryDate == default || itemReq.ExpiryDate.Date <= compareDate.Date)
                    return new ApiResponse<int>(false, $"Product {product.Name} (Lot {itemReq.LotNumber}) is expired or has an invalid expiry date and cannot be accepted.", 0);

                var lineItem = new PurchaseItem
                {
                    ProductId = itemReq.ProductId,
                    Quantity = itemReq.Quantity,
                    UnitPrice = itemReq.UnitPrice,
                    DiscountAmount = itemReq.DiscountAmount,
                    LotNumber = itemReq.LotNumber.Trim(),
                    ExpiryDate = itemReq.ExpiryDate.Date,
                    Taxes = new List<PurchaseItemTax>()
                };

                var lineTotalBeforeTax = (lineItem.Quantity * lineItem.UnitPrice) - lineItem.DiscountAmount;
                if (lineTotalBeforeTax < 0) lineTotalBeforeTax = 0;

                decimal lineTaxAmount = 0;

                foreach (var prodTax in product.ProductTaxes)
                {
                    if (prodTax.Tax.IsActive)
                    {
                        var calculatedTax = Math.Round(lineTotalBeforeTax * (prodTax.Tax.Percentage / 100m), 2);
                        
                        lineItem.Taxes.Add(new PurchaseItemTax
                        {
                            TaxName = prodTax.Tax.Name,
                            TaxPercentage = prodTax.Tax.Percentage,
                            TaxAmount = calculatedTax
                        });

                        lineTaxAmount += calculatedTax;
                    }
                }

                lineItem.TaxAmount = lineTaxAmount;
                lineItem.TotalAmount = lineTotalBeforeTax + lineTaxAmount;

                purchase.SubTotal += lineTotalBeforeTax;
                purchase.TaxAmount += lineTaxAmount;
                purchase.GrandTotal += lineItem.TotalAmount;

                purchase.Items.Add(lineItem);
            }

            await _purchaseRepo.CreatePurchaseAsync(purchase);

            return new ApiResponse<int>(true, "Purchase created successfully.", purchase.Id);
        }

        public async Task<ApiResponse> PostPurchaseAsync(int purchaseId)
        {
            var success = await _purchaseRepo.PostPurchaseAsync(purchaseId);
            if (!success)
                return new ApiResponse(false, "Failed to post purchase. Either it does not exist, is already posted, or an error occurred.");

            return new ApiResponse(true, "Purchase posted successfully. Inventory updated.");
        }

        public async Task<PagedResponseDto<PurchaseDto>> GetAllPurchasesAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var (purchases, totalCount) = await _purchaseRepo.GetPurchasesAsync(pageNumber, pageSize);

            var dtos = purchases.Select(p => new PurchaseDto
            {
                Id = p.Id,
                SupplierId = p.SupplierId,
                InvoiceNumber = p.InvoiceNumber,
                InvoiceDate = p.InvoiceDate,
                PurchaseDate = p.PurchaseDate,
                IsPosted = p.IsPosted,
                SubTotal = p.SubTotal,
                TaxAmount = p.TaxAmount,
                GrandTotal = p.GrandTotal
            }).ToList();

            return new PagedResponseDto<PurchaseDto>(dtos, pageNumber, pageSize, totalCount, "Purchases retrieved successfully");
        }
    }
}
