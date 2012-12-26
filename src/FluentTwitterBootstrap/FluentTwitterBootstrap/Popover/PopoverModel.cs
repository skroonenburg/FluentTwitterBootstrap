namespace FluentTwitterBootstrap.Popover
{
    public class PopoverModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public PopoverPosition Position { get; set; }
    }

    public enum PopoverPosition
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }
}