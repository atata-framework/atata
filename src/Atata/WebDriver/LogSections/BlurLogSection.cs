namespace Atata;

public class BlurLogSection : UIComponentLogSection
{
    public BlurLogSection(UIComponent component)
        : base(component) =>
        Message = $"Blur {component.ComponentFullName}";
}
