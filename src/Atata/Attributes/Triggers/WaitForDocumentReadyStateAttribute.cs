namespace Atata;

/// <summary>
/// Indicates that the waiting should be performed until the document is ready/loaded.
/// By default occurs upon the page object initialization.
/// </summary>
public class WaitForDocumentReadyStateAttribute : WaitForScriptAttribute
{
    public WaitForDocumentReadyStateAttribute(TriggerEvents on = TriggerEvents.Init, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected override string BuildScript<TOwner>(TriggerContext<TOwner> context) =>
        "return document.readyState === 'complete'";
}
