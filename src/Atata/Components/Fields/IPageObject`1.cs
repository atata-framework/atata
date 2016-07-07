namespace Atata
{
    public interface IPageObject<TOwner> : IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        PageTitleDataProvider<TOwner> PageTitle { get; }
        PageUrlDataProvider<TOwner> PageUrl { get; }
    }
}
