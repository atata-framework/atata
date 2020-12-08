using System;

namespace Atata
{
    [Obsolete("Use " + nameof(ValueSetLogSection) + " instead.")] // Obsolete since v1.9.0.
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
