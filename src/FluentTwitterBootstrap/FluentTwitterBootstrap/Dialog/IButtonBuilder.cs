namespace FluentTwitterBootstrap.Dialog
{
    public interface IButtonBuilder
    {
        IButtonBuilder Saying(string buttonText);
        IButtonBuilder NavigateToUrl(string url);
        IButtonBuilder NavigateToAction(string action, string controller = null, object routeValues = null);
        IButtonBuilder AsPostRequest { get; }
        IButtonBuilder ClosesDialog { get; }
        IButtonTypeConfigurer Type { get; } 
    }
}