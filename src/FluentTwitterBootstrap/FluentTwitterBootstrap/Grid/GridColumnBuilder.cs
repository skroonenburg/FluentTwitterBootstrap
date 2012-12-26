using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.WebPages;

namespace FluentTwitterBootstrap.Grid
{
    public class GridColumnBuilder<TModel> : IGridColumnBuilder<TModel>
    {
        private GridColumnModel _columnModel;
        private bool inverse = false;

        public GridColumnBuilder()
        {
            _columnModel = new GridColumnModel();
        }

        public IGridColumnBuilder<TModel> For(string key)
        {
            _columnModel.Key = key;

            return this;
        }

        public IGridColumnBuilder<TModel> For<TProp>(Expression<Func<TModel, TProp>> columnSelector)
        {
            return For(ExpressionHelper.GetExpressionText(columnSelector));
        }

        public IGridColumnBuilder<TModel> Sortable
        {
            get
            {
                _columnModel.IsSortable = !inverse;
                inverse = false;
                return this;
            }
        }

        public IGridColumnBuilder<TModel> SortableAs(string columnName)
        {
            _columnModel.SortableColumnName = columnName;
            
            return Sortable;
        }

        public IGridColumnBuilder<TModel> Not
        {
            get
            {
                inverse = true;
                return this;
            }
        }

        public IGridColumnBuilder<TModel> Titled(string title)
        {
            _columnModel.Name = title;
            return this;
        }

        public IGridColumnBuilder<TModel> Hidden
        {
            get
            {
                _columnModel.IsHidden = true;

                return this;
            }
        }

        public IGridColumnBuilder<TModel> WithContents(string contents)
        {
            _columnModel.Content = contents;

            return this;
        }

        public IGridColumnBuilder<TModel> WithContents(Func<string, HelperResult> contents)
        {
            _columnModel.Content = contents(null).ToString();

            return this;
        }

        public IGridColumnBuilder<TModel> WithWidth(string width)
        {
            _columnModel.Width = width;

            return this;
        }

        internal GridColumnModel Model
        {
            get { return _columnModel; }
        }
    }
}