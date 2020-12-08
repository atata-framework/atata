using System;

namespace Atata
{
    [Obsolete("Use " + nameof(ValueClearLogSection) + " instead.")] // Obsolete since v1.9.0.
    public class DataClearingLogSection : UIComponentLogSection
    {
        public DataClearingLogSection(UIComponent component)
            : base(component)
        {
            Message = $"Clear {component.ComponentFullName}";
        }
    }
}
