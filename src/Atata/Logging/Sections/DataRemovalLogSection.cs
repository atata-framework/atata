using System;

namespace Atata
{
    [Obsolete("Use " + nameof(ValueChangeLogSection) + " instead.")] // Obsolete since v1.9.0.
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
