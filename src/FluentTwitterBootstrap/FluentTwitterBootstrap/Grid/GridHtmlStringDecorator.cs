using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace FluentTwitterBootstrap.Grid
{
    public class GridHtmlStringDecorator<TModel> : GridBuilder<TModel>, IHtmlString
    {
        public string ToHtmlString()
        {
            var table = new TagBuilder("table");

            var gridModel = new
                                {
                                    url = Model.Url,
                                    isPaged = Model.IsPaged,
                                    pageSize = Model.PageSize,
                                    filterByFormId = Model.FilterbyFormId,
                                    onBuildDataUrl = Model.BuildDataUrlCallback,
                                    doNotAutoLoad = Model.DoNotAutoLoad,
                                    loadingContent = Model.LoadingContent,
                                    emptyContent = Model.EmptyContent,
                                    sortColumn = Model.SortColumn,
                                    onShownFunctionCallback = Model.OnShownFunctionCallback,
                                    sortDirection = Model.SortDirection.ToString().ToLowerInvariant(),
                                    title = Model.Title,
                                    columns = Model.Columns.Select(c => new
                                                                            {
                                                                                key = c.Key,
                                                                                name = c.Name,
                                                                                width = c.Width,
                                                                                content = c.Content,
                                                                                hidden = c.IsHidden,
                                                                                isSortable = c.IsSortable,
                                                                                sortableColumnName = string.IsNullOrWhiteSpace(c.SortableColumnName) ? c.Key : c.SortableColumnName
                                                                            })
                                };

            table.Attributes.Add("data-grid", new JavaScriptSerializer().Serialize(gridModel));
            table.GenerateId(Model.Id);

            table.MergeAttributes(new RouteValueDictionary(Model.HtmlAttributes), true);
            
            return table.ToString(TagRenderMode.Normal);
        }
    }
}