#nullable enable

namespace Atata;

public class DoubleClickLogSection : UIComponentLogSection
{
    public DoubleClickLogSection(UIComponent component)
        : base(component) =>
        Message = $"Double-click {component.ComponentFullName}";
}
