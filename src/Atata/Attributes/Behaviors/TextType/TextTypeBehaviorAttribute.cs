#nullable enable

namespace Atata;

/// <summary>
/// Represents the base behavior class for an implementation of control text typing.
/// Responsible for the <see cref="EditableTextField{TValue, TOwner}.Type(string)"/> method action.
/// </summary>
public abstract class TextTypeBehaviorAttribute : MulticastAttribute
{
    /// <summary>
    /// Executes the behavior implementation.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <param name="component">The UI component.</param>
    /// <param name="value">The text value to type.</param>
    public abstract void Execute<TOwner>(IUIComponent<TOwner> component, string value)
        where TOwner : PageObject<TOwner>;
}
