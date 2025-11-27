namespace Atata;

/// <summary>
/// Represents the behavior for control value clearing by trying to execute <see cref="IWebElement.Clear"/> method.
/// If <see cref="InvalidElementStateException"/> occurs, then clears the value by executing
/// <c>HTMLElement.focus(); HTMLElement.value = ''; HTMLElement.dispatchEvent(new Event('change'));</c> JavaScript.
/// </summary>
public class ClearsValueUsingClearMethodOrScriptAttribute : ValueClearBehaviorAttribute
{
    /// <summary>
    /// Gets or sets a value indicating whether to include <c>HTMLElement.focus();</c> JavaScript.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool IncludeFocusScript { get; set; } = true;

    /// <inheritdoc/>
    public override void Execute<TOwner>(IUIComponent<TOwner> component)
    {
        try
        {
            component.Scope.ClearWithLogging(component.Session.Log);
        }
        catch (InvalidElementStateException)
        {
            if (IncludeFocusScript)
                component.Script.FocusSetValueAndDispatchChangeEvent(string.Empty);
            else
                component.Script.SetValueAndDispatchChangeEvent(string.Empty);
        }
    }
}
