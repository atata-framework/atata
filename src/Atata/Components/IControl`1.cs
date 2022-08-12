namespace Atata
{
    public interface IControl<TOwner> : IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        TOwner Click();

        TNavigateTo ClickAndGo<TNavigateTo>(TNavigateTo navigateToPageObject = null, bool? temporarily = null)
            where TNavigateTo : PageObject<TNavigateTo>;

        TOwner Hover();

        TOwner Focus();

        TOwner Blur();

        TOwner DoubleClick();

        TOwner RightClick();
    }
}
