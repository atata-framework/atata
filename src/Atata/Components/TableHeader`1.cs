namespace Atata;

/// <summary>
/// Represents the table header cell control (&lt;th&gt;).
/// Default search finds the first occurring &lt;th&gt; element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("th", ComponentTypeName = "header")]
public class TableHeader<TOwner> : Control<TOwner>
    where TOwner : PageObject<TOwner>
{
}
