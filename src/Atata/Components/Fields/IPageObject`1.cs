namespace Atata
{
    public interface IPageObject<TOwner> : IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        PageTitleValueProvider<TOwner> PageTitle { get; }
        PageUrlValueProvider<TOwner> PageUrl { get; }
    }
}
