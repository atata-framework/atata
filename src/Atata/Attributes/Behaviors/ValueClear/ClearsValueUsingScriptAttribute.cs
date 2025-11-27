namespace Atata;

/// <summary>
/// Represents the behavior for control value clearing by executing
/// <c>HTMLElement.focus(); HTMLElement.value = ''; HTMLElement.dispatchEvent(new Event('change'));</c> JavaScript.
/// </summary>
public class ClearsValueUsingScriptAttribute : ValueClearBehaviorAttribute
{
    /// <summary>
    /// Gets or sets a value indicating whether to include <c>HTMLElement.focus();</c> JavaScript.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool IncludeFocusScript { get; set; } = true;

    /// <inheritdoc/>
    public override void Execute<TOwner>(IUIComponent<TOwner> component)
    {
        if (IncludeFocusScript)
            component.Script.FocusSetValueAndDispatchChangeEvent(string.Empty);
        else
            component.Script.SetValueAndDispatchChangeEvent(string.Empty);
    }
}
