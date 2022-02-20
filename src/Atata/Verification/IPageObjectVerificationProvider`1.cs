namespace Atata
{
    public interface IPageObjectVerificationProvider<out TPageObject> :
        IUIComponentVerificationProvider<TPageObject, TPageObject>
        where TPageObject : PageObject<TPageObject>
    {
    }
}
