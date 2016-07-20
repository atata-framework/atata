namespace Atata
{
    public interface IPageObject<TOwner> : IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        DataProvider<string, TOwner> PageTitle { get; }
        DataProvider<string, TOwner> PageUrl { get; }
    }
}
