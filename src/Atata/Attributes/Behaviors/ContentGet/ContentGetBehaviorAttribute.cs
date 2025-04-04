#nullable enable

namespace Atata;

/// <summary>
/// Represents the base behavior class for getting the component's content.
/// Responsible for the <see cref="UIComponent{TOwner}.Content"/> property value getting.
/// </summary>
public abstract class ContentGetBehaviorAttribute : MulticastAttribute
{
    /// <summary>
    /// Gets the component content.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <param name="component">The component.</param>
    /// <returns>The content.</returns>
    public abstract string Execute<TOwner>(IUIComponent<TOwner> component)
        where TOwner : PageObject<TOwner>;
}
