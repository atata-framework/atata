namespace Atata
{
    internal interface IPageObject
    {
        TOther GoTo<TOther>(TOther pageObject, GoOptions options)
            where TOther : PageObject<TOther>;

        TPageObject SwitchToRoot<TPageObject>(TPageObject rootPageObject = null)
            where TPageObject : PageObject<TPageObject>;
    }
}
