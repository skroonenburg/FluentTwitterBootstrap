using System.ComponentModel;

namespace FluentTwitterBootstrap.Grid
{
    public class PagingOption
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public ListSortDirection SortDirection { get; set; }
    }
}