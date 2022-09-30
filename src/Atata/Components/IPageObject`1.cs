namespace Atata
{
    public interface IPageObject<TOwner> : IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        ValueProvider<string, TOwner> PageTitle { get; }

        ValueProvider<string, TOwner> PageUrl { get; }

        UriProvider<TOwner> PageUri { get; }
    }
}
