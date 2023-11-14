using System;
using System.Collections.Generic;

namespace EcoCentre.Models.Queries
{
    public class PagedCollection<T>
    {
        public PagedCollection(IEnumerable<T> items, int pageSize, int count, int page)
        {
            Items = items;
            PageSize = pageSize;
            Count = count;
            Page = page;
        }
        public IEnumerable<T> Items { get; set; }
        public int Count { get; set; }
        public int PageCount => (int)Math.Ceiling(Count/(double) PageSize);
	    public int PageSize { get; set; }
        public int Page { get; set; }
    }
}