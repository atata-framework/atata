#nullable enable

namespace Atata;

/// <summary>
/// Represents the image input control (<c>&lt;input type="image"&gt;</c>).
/// Default search is performed by <c>alt</c> attribute using <see cref="FindByAltAttribute"/>.
/// </summary>
/// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
/// <seealso cref="ImageInput{TOwner}" />
public class ImageInput<TNavigateTo, TOwner> : ImageInput<TOwner>, INavigable<TNavigateTo, TOwner>
    where TNavigateTo : PageObject<TNavigateTo>
    where TOwner : PageObject<TOwner>
{
}
