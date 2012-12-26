using System;
using System.Linq.Expressions;
using System.Web.WebPages;

namespace FluentTwitterBootstrap.Grid
{
    public interface IGridColumnBuilder<TModel>
    {
        IGridColumnBuilder<TModel> Titled(string title);
        IGridColumnBuilder<TModel> WithContents(string contents);
        IGridColumnBuilder<TModel> WithContents(Func<string, HelperResult> contents);
        IGridColumnBuilder<TModel> WithWidth(string width);
        IGridColumnBuilder<TModel> For(string key);
        IGridColumnBuilder<TModel> For<TProp>(Expression<Func<TModel, TProp>> columnSelector);
        IGridColumnBuilder<TModel> Hidden { get; }
        IGridColumnBuilder<TModel> Sortable { get; }
        IGridColumnBuilder<TModel> Not { get; }
        IGridColumnBuilder<TModel> SortableAs(string columnName);
    }
}