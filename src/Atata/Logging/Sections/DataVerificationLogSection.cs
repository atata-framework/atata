using System.Text;

namespace Atata
{
    public class DataVerificationLogSection : UIComponentLogSection
    {
        public DataVerificationLogSection(UIComponent component, string verificationConstraint)
            : this(component, null, verificationConstraint)
        {
        }

        public DataVerificationLogSection(UIComponent component, string dataProviderName, string verificationConstraint)
            : base(component)
        {
            Message = new StringBuilder($"Verify {component.ComponentFullName}").
                AppendIf(!string.IsNullOrWhiteSpace(dataProviderName), $" {dataProviderName}").
                AppendIf(!string.IsNullOrWhiteSpace(verificationConstraint), $" {verificationConstraint}").
                ToString();
        }
    }
}
