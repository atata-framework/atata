using System.Text;

namespace Atata
{
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
