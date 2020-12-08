namespace Atata
{
    public class ValueChangeLogSection : UIComponentLogSection
    {
        public ValueChangeLogSection(UIComponent component, string changeAction, object value)
            : base(component)
        {
            string valueAsString = SpecialKeys.Replace(Stringifier.ToString(value));
            Message = $"{changeAction} {valueAsString} in {component.ComponentFullName}";
        }
    }
}
