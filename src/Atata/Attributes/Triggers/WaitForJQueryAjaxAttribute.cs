#nullable enable

namespace Atata;

/// <summary>
/// Indicates that the waiting should be performed until the jQuery AJAX is completed.
/// By default occurs after the click.
/// </summary>
public class WaitForJQueryAjaxAttribute : WaitForScriptAttribute
{
    public WaitForJQueryAjaxAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected override string BuildScript<TOwner>(TriggerContext<TOwner> context) =>
        "return jQuery.active == 0";
}
