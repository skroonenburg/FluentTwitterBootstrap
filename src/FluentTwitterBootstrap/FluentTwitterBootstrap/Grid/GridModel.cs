using System.Collections.Generic;
using System.ComponentModel;

namespace FluentTwitterBootstrap.Grid
{
    internal sealed class GridModel
    {
        public GridModel()
        {
            Columns = new List<GridColumnModel>();
            PageSize = 10;
        }

        public IList<GridColumnModel> Columns { get; private set; }
        public object HtmlAttributes { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public int PageSize { get; set; }
        public bool IsPaged { get; set; }
        public string FilterbyFormId { get; set; }
        public string BuildDataUrlCallback { get; set; }
        public string OnShownFunctionCallback { get; set; }
        public bool DoNotAutoLoad { get; set; }
        public string LoadingContent { get; set; }
        public string EmptyContent { get; set; }
        public string SortColumn { get; set; }
        public string Title { get; set; }
        public ListSortDirection SortDirection { get; set; }
    }

    internal sealed class GridColumnModel
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Width { get; set; }
        public string Key { get; set; }
        public bool IsHidden { get; set; }
        public bool IsSortable { get; set; }
        public string SortableColumnName { get; set; }
    }
}