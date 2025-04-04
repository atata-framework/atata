namespace Atata;

/// <summary>
/// Represents the base behavior class for an implementation of the <see cref="EditableTextField{TValue, TOwner}"/> value set.
/// </summary>
public abstract class ValueSetBehaviorAttribute : MulticastAttribute
{
    /// <summary>
    /// Executes the behavior implementation.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <param name="component">The UI component.</param>
    /// <param name="value">The value to set.</param>
    public abstract void Execute<TOwner>(IUIComponent<TOwner> component, string value)
        where TOwner : PageObject<TOwner>;
}
