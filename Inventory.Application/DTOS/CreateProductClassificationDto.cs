using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOS
{
    public class CreateProductClassificationDto
    {
        [Required(ErrorMessage = "Classification name is required.")]
        [MaxLength(100, ErrorMessage = "Classification name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [MaxLength(20, ErrorMessage = "Classification code cannot exceed 20 characters.")]
        public string? Code { get; set; }

        public int? ParentClassificationId { get; set; }
    }
}
