namespace Atata
{
    public class UIComponentClickLogSection : UIComponentLogSection
    {
        public UIComponentClickLogSection(UIComponent component)
            : base(component)
        {
            Message = $"Click {component.ComponentFullName}";
        }
    }
}
