namespace Inventory.Application.DTOS
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsActive { get; set; }
        public bool IsLotTrackingEnabled { get; set; }
        public bool IsExpiryTrackingEnabled { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public int ProductClassificationId { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsLotTrackingEnabled { get; set; }
        public bool IsExpiryTrackingEnabled { get; set; }
    }
}
