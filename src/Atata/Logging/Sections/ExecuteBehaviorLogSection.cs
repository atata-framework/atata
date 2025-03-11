#nullable enable

namespace Atata;

public class ExecuteBehaviorLogSection : UIComponentLogSection
{
    public ExecuteBehaviorLogSection(UIComponent component, MulticastAttribute behavior)
        : base(component)
    {
        string behaviorAsString = Stringifier.ToStringInSimpleStructuredForm(behavior, typeof(MulticastAttribute));

        Message = $"Execute behavior {behaviorAsString} against {component.ComponentFullName}";

        Level = LogLevel.Trace;
    }
}
