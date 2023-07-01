namespace Atata;

/// <summary>
/// Represents any element containing date content.
/// Default search finds the first occurring element.
/// The default format is <c>"d"</c> (short date pattern, e.g. <c>6/15/2009</c>).
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[Format("d")]
public class Date<TOwner> : Content<DateTime?, TOwner>
    where TOwner : PageObject<TOwner>
{
}
