namespace Atata
{
    public class UIComponentSetLogSection : UIComponentLogSection
    {
        public UIComponentSetLogSection(UIComponent component, object value)
            : base(component)
        {
            Message = $"Set \"{value}\" to {component.ComponentFullName}";
        }
    }
}
