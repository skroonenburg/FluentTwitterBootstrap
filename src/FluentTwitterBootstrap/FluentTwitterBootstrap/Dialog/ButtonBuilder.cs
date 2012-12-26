using System.Web;
using System.Web.Mvc;

namespace FluentTwitterBootstrap.Dialog
{
    public class ButtonBuilder : IButtonBuilder, IButtonTypeConfigurer
    {
        private DialogButtonModel _buttonModel;

        public ButtonBuilder(DialogButtonModel buttonModel)
        {
            _buttonModel = buttonModel;
        }

        public IButtonBuilder Saying(string buttonText)
        {
            _buttonModel.Text = buttonText;
            return this;
        }

        public IButtonBuilder NavigateToUrl(string url)
        {
            _buttonModel.Url = url;
            return this;
        }

        public IButtonBuilder NavigateToAction(string action, string controller = null, object routeValues = null)
        {
            return NavigateToUrl(new UrlHelper(HttpContext.Current.Request.RequestContext).Action(action, controller, routeValues));
        }

        public IButtonBuilder AsPostRequest
        {
            get
            {
                _buttonModel.AsPostRequest = true;
                return this;
            }
        }

        public IButtonBuilder ClosesDialog
        {
            get
            {
                _buttonModel.ClosesDialog = true;
                return this;
            }
        }

        public IButtonTypeConfigurer Type
        {
            get { return this; }
        }


        public IButtonBuilder Default
        {
            get
            {
                _buttonModel.Type = ButtonType.Default;
                return this;
            }
        }

        public IButtonBuilder Primary
        {
            get
            {
                _buttonModel.Type = ButtonType.Primary;
                return this;
            }
        }

        public IButtonBuilder Info
        {
            get
            {
                _buttonModel.Type = ButtonType.Info;
                return this;
            }
        }

        public IButtonBuilder Success
        {
            get
            {
                _buttonModel.Type = ButtonType.Success;
                return this;
            }
        }

        public IButtonBuilder Warning
        {
            get
            {
                _buttonModel.Type = ButtonType.Warning;
                return this;
            }
        }

        public IButtonBuilder Danger
        {
            get
            {
                _buttonModel.Type = ButtonType.Danger;
                return this;
            }
        }
    }
}