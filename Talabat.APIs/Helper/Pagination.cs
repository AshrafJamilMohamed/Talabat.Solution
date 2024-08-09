using Talabat.APIs.DTOs;

namespace Talabat.APIs.Helper
{
    public record Pagination<T>
    {
        public Pagination(int pageSize, int pageIndex, IEnumerable<T> mappedProducts, int count)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = mappedProducts;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
