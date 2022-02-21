using System;
using System.Collections.Generic;

namespace ShopBridge.Models
{
    public class PaginatedList<T>
    {
        public PaginatedList(List<T> items, int count, int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
            TotalCount = count;
        }

        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public List<T> Items { get; private set; }
    }
}
