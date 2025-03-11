#nullable enable

namespace Atata;

public class DragAndDropToComponentLogSection : UIComponentLogSection
{
    public DragAndDropToComponentLogSection(UIComponent component, UIComponent targetComponent)
        : base(component)
    {
        TargetComponent = targetComponent;

        Message = $"Drag and drop {component.ComponentFullName} to {targetComponent.ComponentFullName}";
    }

    public UIComponent TargetComponent { get; }
}
