using System;
using System.Web.WebPages;

namespace FluentTwitterBootstrap.Popover
{
    public class PopoverBuilder : IPopoverBuilder, IPopoverPositionConfigurer
    {
        private PopoverModel _model;

        public PopoverBuilder()
        {
            _model = new PopoverModel();
        }

        protected PopoverModel Model
        {
            get { return _model; }
        }

        public IPopoverBuilder WithContent(Func<string, HelperResult> content)
        {
            return WithContent(content(null).ToString());
        }

        public IPopoverBuilder WithContent(string content)
        {
            _model.Content = content;

            return this;
        }

        public IPopoverBuilder Titled(string title)
        {
            _model.Title = title;

            return this;
        }

        public IPopoverPositionConfigurer Positioned
        {
            get { return this; }
        }

        public IPopoverBuilder Left
        {
            get
            {
                _model.Position = PopoverPosition.Left;
                return this;
            }
        }

        public IPopoverBuilder Right
        {
            get
            {
                _model.Position = PopoverPosition.Right;
                return this;
            }
        }

        public IPopoverBuilder Top
        {
            get
            {
                _model.Position = PopoverPosition.Top;
                return this;
            }
        }

        public IPopoverBuilder Bottom
        {
            get
            {
                _model.Position = PopoverPosition.Bottom;
                return this;
            }
        }
    }
}