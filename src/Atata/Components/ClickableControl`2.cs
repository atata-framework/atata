namespace Atata
{
    /// <summary>
    /// Represents any HTML element. By default is being searched by the id attribute.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="ClickableControl{TOwner}" />
    public class ClickableControl<TNavigateTo, TOwner> : ClickableControl<TOwner>, INavigable<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
    }
}
