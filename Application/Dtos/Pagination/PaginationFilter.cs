using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Pagination
{
    public class PaginationFilter
    {
        private const int MaxPageSize = 50;
        private int _pageSize;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public PaginationFilter()
        {
            _pageSize = 10; // default
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            _pageSize = (pageSize > MaxPageSize) ? MaxPageSize : pageSize;
        }
    }
}
