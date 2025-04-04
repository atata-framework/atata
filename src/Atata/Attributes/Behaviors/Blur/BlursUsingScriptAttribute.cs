#nullable enable

namespace Atata;

/// <summary>
/// Represents the behavior for control blurring by executing <c>HTMLElement.blur()</c> JavaScript.
/// </summary>
public class BlursUsingScriptAttribute : BlurBehaviorAttribute
{
    /// <inheritdoc/>
    public override void Execute<TOwner>(IUIComponent<TOwner> component) =>
        component.Script.Blur();
}
