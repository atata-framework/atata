#nullable enable

namespace Atata;

public class ValueSetLogSection : UIComponentLogSection
{
    public ValueSetLogSection(UIComponent component, object? value)
        : base(component)
    {
        string valueAsString = SpecialKeys.Replace(Stringifier.ToString(value));
        Message = $"Set {valueAsString} to {component.ComponentFullName}";
    }
}
