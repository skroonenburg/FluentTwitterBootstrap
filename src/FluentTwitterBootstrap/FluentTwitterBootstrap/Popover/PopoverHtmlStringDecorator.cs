using System.Text;
using System.Web;

namespace FluentTwitterBootstrap.Popover
{
    public class PopoverHtmlStringDecorator : PopoverBuilder, IHtmlString
    {
        public string ToHtmlString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat(@"rel=""popover"" data-popoverposition=""{0}"" data-content=""{1}"" title=""{2}"" ",
                            Model.Position.ToString().ToLowerInvariant(),
                            HttpUtility.HtmlEncode(Model.Content),
                            HttpUtility.HtmlEncode(Model.Title));

            return sb.ToString();
        }
    }
}