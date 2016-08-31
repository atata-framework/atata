namespace Atata
{
    public class UIComponentLogSection : LogSection
    {
        public UIComponentLogSection(UIComponent component)
        {
            Component = component;
        }

        public UIComponent Component { get; private set; }
    }
}
