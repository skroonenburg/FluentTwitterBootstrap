using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.WebPages;

namespace FluentTwitterBootstrap.Grid
{
    public class GridBuilder<TModel> : IGridBuilder<TModel>, ISortDirectionConfigurer<TModel>
    {
        private GridModel _gridModel;

        public GridBuilder()
        {
            _gridModel = new GridModel();
        }

        public IGridBuilder<TModel> PopulateFrom(string url)
        {
            _gridModel.Url = url;

            return this;
        }

        public ISortDirectionConfigurer<TModel> SortedBy(string column)
        {
            _gridModel.SortColumn = column;

            return this;
        }

        public IGridBuilder<TModel> Titled(Func<object, HelperResult> titleExpression)
        {
            _gridModel.Title = titleExpression(null).ToHtmlString();

            return this;
        }

        public IGridBuilder<TModel> Titled(string title)
        {
            _gridModel.Title = title;

            return this;
        }

        public ISortDirectionConfigurer<TModel> SortedBy<TProp>(Expression<Func<TModel, TProp>> columnExpression)
        {
            return SortedBy(ExpressionHelper.GetExpressionText(columnExpression));
        }

        public IGridBuilder<TModel> WithPagesOf(int pageSize)
        {
            _gridModel.PageSize = pageSize;
            _gridModel.IsPaged = true;

            return this;
        }

        public IGridBuilder<TModel> WithLoadingContent(Func<object, HelperResult> htmlContent)
        {
            return WithLoadingContent(htmlContent(null).ToHtmlString());
        }

        public IGridBuilder<TModel> WithLoadingContent(string htmlContent)
        {
            _gridModel.LoadingContent = htmlContent;

            return this;
        }

        public IGridBuilder<TModel> WithEmptyContent(Func<object, HelperResult> htmlContent)
        {
            return WithEmptyContent(htmlContent(null).ToHtmlString());
        }

        public IGridBuilder<TModel> WithEmptyContent(string htmlContent)
        {
            _gridModel.EmptyContent = htmlContent;

            return this;
        }

        public IGridBuilder<TModel> DoNotAutoLoad
        {
            get
            {
                _gridModel.DoNotAutoLoad = true;

                return this;
            }
        }

        public IGridBuilder<TModel> HidePagingControls()
        {
            _gridModel.IsPaged = false;

            return this;
        }

        public IGridBuilder<TModel> OnBuildDataUrl(string buildDataUrlCallback)
        {
            _gridModel.BuildDataUrlCallback = buildDataUrlCallback;

            return this;
        }

        public IGridBuilder<TModel> OnShownFunction(string onShownFunctionCallback)
        {
            _gridModel.OnShownFunctionCallback = onShownFunctionCallback;

            return this;
        }

        public IGridBuilder<TModel> FilterByForm(string formId)
        {
            _gridModel.FilterbyFormId = formId;

            return this;
        }

        public IGridBuilder<TModel> WithId(string gridId)
        {
            _gridModel.Id = gridId;

            return this;
        }

        public IGridBuilder<TModel> WithHtmlAttributes(object htmlAttributes)
        {
            _gridModel.HtmlAttributes = htmlAttributes;

            return this;
        }


        public IGridBuilder<TModel> Columns(params Func<IGridColumnBuilder<TModel>, IGridColumnBuilder<TModel>>[] gridColumnBuilder)
        {
            var newColumns = gridColumnBuilder.Select(x => { var b = new GridColumnBuilder<TModel>(); x.Invoke(b); return b.Model; });

            foreach (var newColumn in newColumns)
            {
                _gridModel.Columns.Add(newColumn);
            }

            return this;
        }

        internal GridModel Model
        {
            get { return _gridModel; }
        }

        public IGridBuilder<TModel> Ascending
        {
            get
            {
                _gridModel.SortDirection = ListSortDirection.Ascending;
                return this;
            }
        }

        public IGridBuilder<TModel> Descending
        {
            get
            {
                _gridModel.SortDirection = ListSortDirection.Descending;
                return this;
            }
        }
    }
}