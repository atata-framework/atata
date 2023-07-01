namespace Atata;

/// <summary>
/// Represents the behavior for control value clearing by executing
/// <c>HTMLElement.value = ''; HTMLElement.dispatchEvent(new Event('change'));</c> JavaScript.
/// </summary>
public class ClearsValueUsingScriptAttribute : ValueClearBehaviorAttribute
{
    /// <inheritdoc/>
    public override void Execute<TOwner>(IUIComponent<TOwner> component) =>
        component.Script.SetValueAndDispatchChangeEvent(string.Empty);
}
