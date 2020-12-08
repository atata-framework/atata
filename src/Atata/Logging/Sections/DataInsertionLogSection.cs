using System;
using System.Text;

namespace Atata
{
    [Obsolete("Use " + nameof(ValueChangeLogSection) + " instead.")] // Obsolete since v1.9.0.
    public class DataInsertionLogSection : UIComponentLogSection
    {
        public DataInsertionLogSection(UIComponent component, object value, int? index = null)
            : base(component)
        {
            Message = new StringBuilder($"Insert \"{value}\" to {component.ComponentFullName}").
                AppendIf(index.HasValue, $" at index {index}").
                ToString();
        }
    }
}
