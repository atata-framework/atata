namespace Atata;

public class ExecuteTriggerLogSection : UIComponentLogSection
{
    public ExecuteTriggerLogSection(UIComponent component, TriggerAttribute trigger, TriggerEvents triggerEvent)
        : base(component)
    {
        string triggerAsString = Stringifier.ToStringInSimpleStructuredForm(trigger, typeof(TriggerAttribute));

        Message = $"Execute trigger {triggerAsString} on {triggerEvent} against {component.ComponentFullName}";

        Level = LogLevel.Trace;
    }
}
