using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOS
{
    public class CreateProductClassificationDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        public string? Code { get; set; }

        public int? ParentClassificationId { get; set; }
    }
}
