using System.Text;

namespace Atata
{
    /// <summary>
    /// Represents the log section of value verification.
    /// </summary>
    public class ValueVerificationLogSection : LogSection
    {
        public ValueVerificationLogSection(
            string verificationKind,
            string dataProviderName,
            string verificationConstraint)
        {
            var builder = new StringBuilder(verificationKind ?? "Verify")
                .Append($": {dataProviderName ?? "value"}");

            if (!string.IsNullOrWhiteSpace(verificationConstraint))
                builder.Append($" {verificationConstraint}");

            Message = builder.ToString();
        }
    }
}
