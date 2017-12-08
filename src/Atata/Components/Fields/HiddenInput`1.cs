namespace Atata
{
    /// <summary>
    /// Represents the hidden input control (&lt;input type="hidden"&gt;) with text data.
    /// Default search finds the first occurring &lt;input type="hidden"&gt; element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class HiddenInput<TOwner> : HiddenInput<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
