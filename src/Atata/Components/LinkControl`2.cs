namespace Atata
{
    /// <summary>
    /// Represents the link control (&lt;a&gt;). Default search is performed by the content.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="LinkControl{TOwner}" />
    public class LinkControl<TNavigateTo, TOwner> : LinkControl<TOwner>, INavigable<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
    }
}
