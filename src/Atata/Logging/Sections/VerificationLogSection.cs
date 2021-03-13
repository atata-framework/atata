using System.Text;

namespace Atata
{
    /// <summary>
    /// Represents the log section of component or component value verification.
    /// </summary>
    public class VerificationLogSection : UIComponentLogSection
    {
        public VerificationLogSection(UIComponent component, string verificationConstraint)
            : this(null, component, null, verificationConstraint)
        {
        }

        public VerificationLogSection(UIComponent component, string dataProviderName, string verificationConstraint)
            : this(null, component, dataProviderName, verificationConstraint)
        {
        }

        public VerificationLogSection(string verificationKind, UIComponent component, string verificationConstraint)
            : this(verificationKind, component, null, verificationConstraint)
        {
        }

        public VerificationLogSection(string verificationKind, UIComponent component, string dataProviderName, string verificationConstraint)
            : base(component)
        {
            Message = new StringBuilder(verificationKind ?? "Verify").
                Append(':').
                AppendIf(!component.GetType().IsSubclassOfRawGeneric(typeof(PageObject<>)), $" {component.ComponentFullName}").
                AppendIf(!string.IsNullOrWhiteSpace(dataProviderName), $" {dataProviderName}").
                AppendIf(!string.IsNullOrWhiteSpace(verificationConstraint), $" {verificationConstraint}").
                ToString();

            Level = LogLevel.Info;
        }
    }
}
