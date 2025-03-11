#nullable enable

namespace Atata;

public class ValueClearLogSection : UIComponentLogSection
{
    public ValueClearLogSection(UIComponent component)
        : base(component) =>
        Message = $"Clear {component.ComponentFullName}";
}
