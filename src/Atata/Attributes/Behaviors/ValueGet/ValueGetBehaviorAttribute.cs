namespace Atata;

/// <summary>
/// Represents the base behavior class for an implementation of the <see cref="EditableTextField{TValue, TOwner}"/> value getting.
/// </summary>
public abstract class ValueGetBehaviorAttribute : MulticastAttribute
{
    /// <summary>
    /// Executes the behavior implementation.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <param name="component">The UI component.</param>
    /// <returns>The value.</returns>
    public abstract string Execute<TOwner>(IUIComponent<TOwner> component)
        where TOwner : PageObject<TOwner>;
}
