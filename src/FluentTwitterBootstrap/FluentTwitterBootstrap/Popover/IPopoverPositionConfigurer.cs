namespace FluentTwitterBootstrap.Popover
{
    public interface IPopoverPositionConfigurer
    {
        IPopoverBuilder Left { get; }
        IPopoverBuilder Right { get; }
        IPopoverBuilder Top { get; }
        IPopoverBuilder Bottom { get; }
    }
}