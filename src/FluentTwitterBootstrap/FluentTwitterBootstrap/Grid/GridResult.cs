using System.Collections.Generic;

namespace FluentTwitterBootstrap.Grid
{
    public class GridResult<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}