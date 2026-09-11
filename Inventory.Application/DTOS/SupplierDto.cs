using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.DTOS
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    public class CreateSupplierDto
    {
        [Required(ErrorMessage = "Supplier name is required.")]
        [MaxLength(200, ErrorMessage = "Supplier name cannot exceed 200 characters.")]
        public string Name { get; set; } = null!;

        [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string? Phone { get; set; }

        [MaxLength(100, ErrorMessage = "Email address cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
    }
}
