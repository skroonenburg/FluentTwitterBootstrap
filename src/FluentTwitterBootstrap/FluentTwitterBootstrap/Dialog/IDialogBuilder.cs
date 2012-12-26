using System;
using System.Web.WebPages;

namespace FluentTwitterBootstrap.Dialog
{
    public interface IDialogBuilder
    {
        IDialogBuilder Saying(string text);
        IDialogBuilder Saying(Func<string, HelperResult> content);
        IDialogBuilder Titled(string title);
        IDialogBuilder CloseOnEscape { get; }
        IDialogBuilder ShowBackDrop { get; }
        IDialogBuilder Not { get; }
        IDialogBuilder WithButtons(params Func<IButtonBuilder, IButtonBuilder>[] buttonConfigurations);
        IDialogBuilder WithContentFromAction(string actionName, string controllerName = null, object routeValues = null);
        IDialogBuilder WithContentFromUrl(string url);
    }
}