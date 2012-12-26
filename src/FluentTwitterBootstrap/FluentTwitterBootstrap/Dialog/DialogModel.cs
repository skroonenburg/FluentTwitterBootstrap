using System.Collections.Generic;

namespace FluentTwitterBootstrap.Dialog
{
    public class DialogModel
    {
        public DialogModel()
        {
            Buttons = new List<DialogButtonModel>();
            CloseOnEscape = true;
            ShowBackDrop = true;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public string ContentUrl { get; set; }
        public bool CloseOnEscape { get; set; }
        public bool ShowBackDrop { get; set; }
        public List<DialogButtonModel> Buttons { get; set; }
        public string LoadingContent { get; set; }
    }
}