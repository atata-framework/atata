namespace Atata
{
    public class DataSettingLogSection : UIComponentLogSection
    {
        public DataSettingLogSection(UIComponent component, object value)
            : base(component)
        {
            Message = $"{ActionText} \"{value}\" to {component.ComponentFullName}";
        }

        public string ActionText { get; set; } = "Set";
    }
}
