using System.Collections.Generic;
using System;

namespace Inventory.Application.DTOS
{
    public class PagedResponseDto<T> : ApiResponse<IEnumerable<T>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponseDto(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords, string message = null) 
            : base(true, message ?? "Data retrieved successfully", data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = totalRecords > 0 ? (int)Math.Ceiling(totalRecords / (double)pageSize) : 0;
        }
    }
}
