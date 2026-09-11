namespace Inventory.Application.DTOS
{
    public class CreateProductClassificationDto
    {
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public int? ParentClassificationId { get; set; }
    }
}
