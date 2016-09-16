namespace Atata
{
    public interface IControl<TOwner> : IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        TOwner Click();

        TOwner Hover();

        TOwner Focus();

        TOwner DoubleClick();

        TOwner RightClick();
    }
}
