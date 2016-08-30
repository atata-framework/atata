using System.Text;

namespace Atata
{
    public class UIComponentVerificationLogSection : UIComponentLogSection
    {
        public UIComponentVerificationLogSection(UIComponent component, string verificationConstraint)
            : this(component, null, verificationConstraint)
        {
        }

        public UIComponentVerificationLogSection(UIComponent component, string dataProviderName, string verificationConstraint)
            : base(component)
        {
            Message = new StringBuilder($"Verify {component.ComponentFullName} ").
                Append(dataProviderName).
                AppendIf(!string.IsNullOrEmpty(dataProviderName), " ").
                Append(verificationConstraint).
                ToString();
        }
    }
}
