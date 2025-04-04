#nullable enable

namespace Atata;

/// <summary>
/// Represents the base behavior class for control focus implementation.
/// Responsible for the <see cref="Control{TOwner}.Focus"/> method action.
/// </summary>
public abstract class FocusBehaviorAttribute : MulticastAttribute
{
    /// <summary>
    /// Executes the behavior implementation.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <param name="component">The UI component.</param>
    public abstract void Execute<TOwner>(IUIComponent<TOwner> component)
        where TOwner : PageObject<TOwner>;
}
