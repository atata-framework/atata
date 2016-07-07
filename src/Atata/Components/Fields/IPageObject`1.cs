namespace Atata
{
    public interface IPageObject<TOwner> : IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        UIComponentDataProvider<string, TOwner> PageTitle { get; }
        UIComponentDataProvider<string, TOwner> PageUrl { get; }
    }
}
