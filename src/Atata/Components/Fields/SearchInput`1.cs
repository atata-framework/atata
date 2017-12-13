namespace Atata
{
    /// <summary>
    /// Represents the search input control (&lt;input type="search"&gt;).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='search']")]
    public class SearchInput<TOwner> : Input<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
