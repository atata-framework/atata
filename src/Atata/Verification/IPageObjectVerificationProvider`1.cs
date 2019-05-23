namespace Atata
{
    public interface IPageObjectVerificationProvider<TPageObject> :
        IUIComponentVerificationProvider<TPageObject, TPageObject>
        where TPageObject : PageObject<TPageObject>
    {
    }
}
