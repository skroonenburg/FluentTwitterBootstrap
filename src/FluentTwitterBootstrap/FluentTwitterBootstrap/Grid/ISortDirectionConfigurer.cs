namespace FluentTwitterBootstrap.Grid
{
    public interface ISortDirectionConfigurer<TModel>
    {
        IGridBuilder<TModel> Ascending { get; }
        IGridBuilder<TModel> Descending { get; }
    }
}
