namespace Inventory.Application.DTOS
{
    public class TaxDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Percentage { get; set; }
        public string DisplayLabel => $"{Name} ({Percentage}%)";
    }
}
