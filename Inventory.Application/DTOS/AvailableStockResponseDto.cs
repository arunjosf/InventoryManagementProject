using System;
using System.Collections.Generic;

namespace Inventory.Application.DTOS
{
    public class AvailableStockResponseDto
    {
        public string Product { get; set; } = string.Empty;
        public int TotalAvailableQuantity { get; set; }
        public List<FifoStockDto> FifoStockQueue { get; set; } = new();
    }

    public class FifoStockDto
    {
        public string? LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
