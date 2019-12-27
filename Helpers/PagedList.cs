using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            this.AddRange(items);
        }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public static async Task<PagedList<T>> CreateListAsync(IQueryable<T> source, int pageSize, int pageNumber)
        {
            var totalCount = await source.CountAsync();

            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}