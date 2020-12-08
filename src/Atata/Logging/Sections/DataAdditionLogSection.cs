using System;

namespace Atata
{
    [Obsolete("Use " + nameof(ValueChangeLogSection) + " instead.")] // Obsolete since v1.9.0.
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
