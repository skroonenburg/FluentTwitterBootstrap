using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace FluentTwitterBootstrap.Dialog
{
    public class DialogBuilder : IDialogBuilder
    {
        private bool _state = true;
        protected DialogModel _dialogModel;

        public DialogBuilder()
            : this(new DialogModel())
        {
        }

        public DialogBuilder(DialogModel dialogModel)
        {
            _dialogModel = dialogModel;
        }

        public IDialogBuilder Saying(string text)
        {
            _dialogModel.Content = text;
            return this;
        }

        public IDialogBuilder Saying(Func<string, HelperResult> content)
        {
            return Saying(content(null).ToString());
        }

        public IDialogBuilder Titled(string title)
        {
            _dialogModel.Title = title;
            return this;
        }

        public IDialogBuilder CloseOnEscape
        {
            get
            {
                _dialogModel.CloseOnEscape = _state;
                return this;
            }
        }

        public IDialogBuilder ShowBackDrop
        {
            get
            {
                _dialogModel.ShowBackDrop = _state;
                return this;
            }
        }

        public IDialogBuilder Not
        {
            get
            {
                _state = !_state;
                return this;
            }
        }

        public IDialogBuilder WithButtons(params Func<IButtonBuilder, IButtonBuilder>[] buttonConfigurations)
        {
            foreach (var buttonConfiguration in buttonConfigurations)
            {
                var buttonModel = new DialogButtonModel();
                buttonConfiguration(new ButtonBuilder(buttonModel));
                _dialogModel.Buttons.Add(buttonModel);
            }

            return this;
        }

        public IDialogBuilder WithContentFromAction(string actionName, string controllerName = null, object routeValues = null)
        {
            return WithContentFromUrl(new UrlHelper(HttpContext.Current.Request.RequestContext).Action(actionName, controllerName, routeValues));
        }

        public IDialogBuilder WithContentFromUrl(string url)
        {
            _dialogModel.ContentUrl = url;

            return this;
        }

        public IDialogBuilder WithLoadingContent(Func<object, HelperResult> loadingContent)
        {
            return WithLoadingContent(loadingContent(null).ToString());
        }

        public IDialogBuilder WithLoadingContent(string loadingContent)
        {
            _dialogModel.LoadingContent = loadingContent;

            return this;
        }
    }
}