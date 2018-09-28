namespace Atata
{
    public class UIComponentLogSection : LogSection
    {
        public UIComponentLogSection(UIComponent component)
        {
            Component = component;

            TraceLogAttribute traceLogAttribute = component.Metadata.Get<TraceLogAttribute>(x => x.At(AttributeLevels.DeclaredAndComponent));

            if (traceLogAttribute != null)
                Level = LogLevel.Trace;
        }

        public UIComponent Component { get; private set; }
    }
}
