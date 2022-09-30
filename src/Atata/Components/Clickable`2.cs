namespace Atata
{
    /// <summary>
    /// Represents any HTML element.
    /// Default search finds the first occurring element.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Clickable<TNavigateTo, TOwner> : Clickable<TOwner>, INavigable<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
    }
}
