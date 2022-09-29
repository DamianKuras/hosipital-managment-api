using hosipital_managment_api.Models;
using Microsoft.EntityFrameworkCore;

namespace hosipital_managment_api.Extensions
{

    public class PagedList<T> : List<T>
    {
        public PagingMetadata PagingMetadata { get; private set; }

        public PagedList(List<T> items, PagingMetadata pagingMetadata)
        {
            PagingMetadata = pagingMetadata;
            AddRange(items);
        }
        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            
            int totalPages = count > 0 ?
                (int)Math.Ceiling(count / (double)pageSize) : 0;
            PagingMetadata metadata = new PagingMetadata()
            {
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalCount = count
            };
            if (pageNumber<0 || pageNumber > totalPages)
            {
                return new PagedList<T>(new List<T>(), metadata);
            }
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items,metadata);
        }
    }
}
