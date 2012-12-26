using System.Web;

namespace FluentTwitterBootstrap.Dialog
{
    public class DialogHtmlStringDecorator : DialogBuilder, IHtmlString
    {
        public string ToHtmlString()
        {
            return DialogJsonRenderer.Render(_dialogModel);
        }
    }
}