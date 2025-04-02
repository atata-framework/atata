#nullable enable

namespace Atata;

/// <summary>
/// Represents the hidden input control (<c>&lt;input type="hidden"&gt;</c>) with text data.
/// Default search finds the first occurring <c>&lt;input type="hidden"&gt;</c> element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public class HiddenInput<TOwner> : HiddenInput<string, TOwner>
    where TOwner : PageObject<TOwner>
{
}
