namespace Atata
{
    public class DataRemovalLogSection : UIComponentLogSection
    {
        public DataRemovalLogSection(UIComponent component, object value)
            : base(component)
        {
            Message = $"{ActionText} \"{value}\" from {component.ComponentFullName}";
        }

        public string ActionText { get; set; } = "Remove";
    }
}
