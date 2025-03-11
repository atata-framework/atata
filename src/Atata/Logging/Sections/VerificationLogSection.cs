#nullable enable

namespace Atata;

/// <summary>
/// Represents the log section of verification.
/// </summary>
public class VerificationLogSection : LogSection
{
    public VerificationLogSection(
        string verificationKind,
        string providerName,
        string? verificationConstraint = null)
    {
        StringBuilder builder = new($"{verificationKind}: {providerName}");

        if (verificationConstraint?.Length > 0)
            builder.Append(' ').Append(verificationConstraint);

        Message = builder.ToString();
    }
}
