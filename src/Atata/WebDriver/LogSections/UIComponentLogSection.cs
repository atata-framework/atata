namespace Atata;

public class UIComponentLogSection : LogSection
{
    public UIComponentLogSection(UIComponent component)
    {
        Component = component;

        if (component.Metadata.Contains<TraceLogAttribute>())
            Level = LogLevel.Trace;
    }

    public UIComponent Component { get; private set; }
}
