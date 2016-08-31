namespace Atata
{
    public class DataAdditionLogSection : UIComponentLogSection
    {
        public DataAdditionLogSection(UIComponent component, object value)
            : base(component)
        {
            Message = $"{ActionText} \"{value}\" to {component.ComponentFullName}";
        }

        public string ActionText { get; set; } = "Add";
    }
}
