using System.Text;

namespace Atata
{
    public class VerificationLogSection : UIComponentLogSection
    {
        public VerificationLogSection(UIComponent component, string verificationConstraint)
            : this(component, null, verificationConstraint)
        {
        }

        public VerificationLogSection(UIComponent component, string dataProviderName, string verificationConstraint)
            : base(component)
        {
            Message = new StringBuilder($"Verify").
                AppendIf(!component.GetType().IsSubclassOfRawGeneric(typeof(PageObject<>)), $" {component.ComponentFullName}").
                AppendIf(!string.IsNullOrWhiteSpace(dataProviderName), $" {dataProviderName}").
                AppendIf(!string.IsNullOrWhiteSpace(verificationConstraint), $" {verificationConstraint}").
                ToString();
        }
    }
}
