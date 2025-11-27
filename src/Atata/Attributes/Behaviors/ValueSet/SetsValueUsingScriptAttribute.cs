namespace Atata;

/// <summary>
/// Represents the behavior for control value set by executing
/// <c>HTMLElement.focus(); HTMLElement.value = '{value}'; HTMLElement.dispatchEvent(new Event('change'));</c> JavaScript.
/// </summary>
public class SetsValueUsingScriptAttribute : ValueSetBehaviorAttribute
{
    /// <summary>
    /// Gets or sets a value indicating whether to include <c>HTMLElement.focus();</c> JavaScript.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool IncludeFocusScript { get; set; } = true;

    /// <inheritdoc/>
    public override void Execute<TOwner>(IUIComponent<TOwner> component, string value)
    {
        if (IncludeFocusScript)
            component.Script.FocusSetValueAndDispatchChangeEvent(value);
        else
            component.Script.SetValueAndDispatchChangeEvent(value);
    }
}
