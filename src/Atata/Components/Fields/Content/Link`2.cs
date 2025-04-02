#nullable enable

namespace Atata;

/// <summary>
/// Represents the link control (<c>&lt;a&gt;</c>).
/// Default search is performed by the content.
/// </summary>
/// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
/// <seealso cref="Link{TOwner}" />
public class Link<TNavigateTo, TOwner> : Link<TOwner>, INavigable<TNavigateTo, TOwner>
    where TNavigateTo : PageObject<TNavigateTo>
    where TOwner : PageObject<TOwner>
{
}
