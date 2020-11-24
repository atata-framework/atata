namespace Atata
{
    public class ValueTypeLogSection : UIComponentLogSection
    {
        public ValueTypeLogSection(UIComponent component, string value)
            : base(component)
        {
            Message = $"Type \"{value}\" in {component.ComponentFullName}";
        }
    }
}
