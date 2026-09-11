using System;

namespace Inventory.Application.DTOS
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsPosted { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
