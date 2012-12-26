using System.Web;
using System.Web.Mvc;

namespace FluentTwitterBootstrap.Dialog
{
    public class DialogCommand
    {
        public static JsonResult Close()
        {
            return AsJson(new DialogCommandResult
                              {
                                  command = "close"
                              });
        }

        public static JsonResult RedirectToAction(string action, string controller = null, object routeValues = null)
        {
            return RedirectToUrl(new UrlHelper(HttpContext.Current.Request.RequestContext).Action(action, controller, routeValues));
        }

        public static JsonResult RedirectToUrl(string url)
        {
            return AsJson(new DialogCommandResult
            {
                command = "redirect",
                parameter = url
            });
        }

        private static JsonResult AsJson(DialogCommandResult dialogCommand)
        {
            return new JsonResult
                       {
                           ContentType = "application/json",
                           Data = dialogCommand
                       };
        }
    }

    public class DialogCommandResult
    {
        public string command { get; set; }
        public string parameter { get; set; }
    }
}