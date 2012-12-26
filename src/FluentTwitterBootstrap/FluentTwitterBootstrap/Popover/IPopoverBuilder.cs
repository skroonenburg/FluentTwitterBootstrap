using System;
using System.Web.WebPages;

namespace FluentTwitterBootstrap.Popover
{
    public interface IPopoverBuilder
    {
        IPopoverPositionConfigurer Positioned { get; }
        IPopoverBuilder WithContent(Func<string, HelperResult> content);
        IPopoverBuilder WithContent(string content);
        IPopoverBuilder Titled(string title);
    }
}