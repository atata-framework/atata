#nullable enable

namespace Atata;

/// <summary>
/// Specifies whether to temporarily navigate to page object.
/// </summary>
public class GoTemporarilyAttribute : MulticastAttribute
{
    public GoTemporarilyAttribute(bool isTemporarily = true) =>
        IsTemporarily = isTemporarily;

    public bool IsTemporarily { get; }
}
