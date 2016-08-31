namespace Atata
{
    public class DataClearingLogSection : UIComponentLogSection
    {
        public DataClearingLogSection(UIComponent component)
            : base(component)
        {
            Message = $"Clear {component.ComponentFullName}";
        }
    }
}
