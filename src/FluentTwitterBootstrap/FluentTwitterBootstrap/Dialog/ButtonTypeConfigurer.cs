namespace FluentTwitterBootstrap.Dialog
{
    public interface IButtonTypeConfigurer
    {
        IButtonBuilder Default { get; }
        IButtonBuilder Primary { get; }
        IButtonBuilder Info { get; }
        IButtonBuilder Success { get; }
        IButtonBuilder Warning { get; }
        IButtonBuilder Danger { get; }
    }
}