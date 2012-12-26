using System.Web.Mvc;
using FluentTwitterBootstrap.Dialog;
using FluentTwitterBootstrap.Grid;
using FluentTwitterBootstrap.Popover;

namespace FluentTwitterBootstrap
{
    public static class BootstrapHtmlHelpers
    {
        public static IDialogBuilder Dialog(this HtmlHelper instance, string title)
        {
            return new DialogHtmlStringDecorator().Titled(title);
        }

        public static IGridBuilder<TModel> GridFor<TModel>(this HtmlHelper instance, string gridId = null)
        {
            return new GridHtmlStringDecorator<TModel>().WithId(gridId);
        }

        public static IPopoverBuilder Popover(this HtmlHelper instance)
        {
            return new PopoverHtmlStringDecorator();
        }

        public static IPopoverBuilder Popover(this HtmlHelper instance, string title)
        {
            return Popover(instance).Titled(title);
        }
    }
}