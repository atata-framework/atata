namespace Atata.WebDriver;

public interface IPageObjectVerificationProvider<out TPageObject> :
    IUIComponentVerificationProvider<TPageObject, TPageObject>
    where TPageObject : PageObject<TPageObject>
{
}
