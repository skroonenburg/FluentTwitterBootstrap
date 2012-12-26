using System.Linq;
using System.Web.Script.Serialization;

namespace FluentTwitterBootstrap.Dialog
{
    public static class DialogJsonRenderer
    {
        public static string Render(DialogModel model)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            var jsonModel = new
                                {
                                    closeOnEscape = model.CloseOnEscape,
                                    content = model.Content,
                                    contentUrl = model.ContentUrl,
                                    showBackDrop = model.ShowBackDrop,
                                    title = model.Title,
                                    loadingContnet = model.LoadingContent,
                                    buttons = model.Buttons.Select(x => new
                                                                 {
                                                                     text = x.Text,
                                                                     url = x.Url,
                                                                     asPost = x.AsPostRequest,
                                                                     closesDialog = x.ClosesDialog,
                                                                     type = x.Type.ToString().ToLowerInvariant()
                                                                 })
                                };

            return string.Format("data-dialogHelper='{0}'", javascriptSerializer.Serialize(jsonModel));
        }
    }
}