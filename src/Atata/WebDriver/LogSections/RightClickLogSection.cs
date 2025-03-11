#nullable enable

namespace Atata;

public class RightClickLogSection : UIComponentLogSection
{
    public RightClickLogSection(UIComponent component)
        : base(component) =>
        Message = $"Right-click {component.ComponentFullName}";
}
