using System;
using System.Linq.Expressions;
using System.Web.WebPages;

namespace FluentTwitterBootstrap.Grid
{
    public interface IGridBuilder<TModel>
    {
        IGridBuilder<TModel> Columns(params Func<IGridColumnBuilder<TModel>, IGridColumnBuilder<TModel>>[] gridColumnBuilder);
        IGridBuilder<TModel> PopulateFrom(string url);
        IGridBuilder<TModel> WithId(string gridId);
        IGridBuilder<TModel> WithHtmlAttributes(object htmlAttributes);
        IGridBuilder<TModel> WithPagesOf(int pageSize);
        IGridBuilder<TModel> FilterByForm(string formId);
        IGridBuilder<TModel> HidePagingControls();
        IGridBuilder<TModel> OnBuildDataUrl(string buildDataUrlCallback);
        IGridBuilder<TModel> OnShownFunction(string onShowFunctionCallback);
        IGridBuilder<TModel> DoNotAutoLoad { get; }
        IGridBuilder<TModel> WithLoadingContent(Func<object, HelperResult> htmlContent);
        IGridBuilder<TModel> WithLoadingContent(string htmlContent);
        IGridBuilder<TModel> WithEmptyContent(Func<object, HelperResult> htmlContent);
        IGridBuilder<TModel> WithEmptyContent(string htmlContent);
        ISortDirectionConfigurer<TModel> SortedBy(string column);
        ISortDirectionConfigurer<TModel> SortedBy<TProp>(Expression<Func<TModel, TProp>> columnExpression);
        IGridBuilder<TModel> Titled(string title);
        IGridBuilder<TModel> Titled(Func<object, HelperResult> titleExpression);
    }
}