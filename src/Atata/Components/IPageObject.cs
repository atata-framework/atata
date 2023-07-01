namespace Atata;

internal interface IPageObject
{
    void SwitchToWindow(string windowHandle);

    TPageObject SwitchToRoot<TPageObject>(TPageObject rootPageObject = null)
        where TPageObject : PageObject<TPageObject>;

    void DeInit();
}
