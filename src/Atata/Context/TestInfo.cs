#nullable enable

namespace Atata;

/// <summary>
/// Represents the basic test information.
/// </summary>
public sealed class TestInfo : IEquatable<TestInfo>
{
    public TestInfo(Type? suiteType, string? suiteName = null, string? suiteGroupName = null)
        : this(null, suiteType, suiteName, suiteGroupName)
    {
    }

    public TestInfo(string? name, Type? suiteType, string? suiteName = null, string? suiteGroupName = null)
    {
        Name = name;
        NameSanitized = name?.SanitizeForFileName(AtataPathTemplateStringFormatter.CharToReplaceWith);

        SuiteName = suiteName ?? suiteType?.Name;
        SuiteType = suiteType;
        SuiteGroupName = suiteGroupName;

        if (suiteType is not null)
        {
            Namespace = suiteType.Namespace;
        }
        else if (suiteName is not null)
        {
            int lastIndexOfPeriod = suiteName.LastIndexOf('.');

            if (lastIndexOfPeriod > 0 && lastIndexOfPeriod < suiteName.Length - 1)
            {
                Namespace = suiteName[..lastIndexOfPeriod];
                SuiteName = suiteName[(lastIndexOfPeriod + 1)..];
            }
        }

        SuiteNameSanitized = SuiteName?.SanitizeForFileName(AtataPathTemplateStringFormatter.CharToReplaceWith);

        FullName = BuildFullName();
    }

    /// <summary>
    /// Gets the name of the test.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Gets the name of the test sanitized for file path/name.
    /// </summary>
    public string? NameSanitized { get; }

    /// <summary>
    /// Gets the name of the test suite (class).
    /// </summary>
    public string? SuiteName { get; }

    /// <summary>
    /// Gets the name of the test suite (class) sanitized for file path/name.
    /// </summary>
    public string? SuiteNameSanitized { get; }

    /// <summary>
    /// Gets the test suite (class) type.
    /// </summary>
    public Type? SuiteType { get; }

    /// <summary>
    /// Gets the name of the test suite group (collection fixture).
    /// </summary>
    public string? SuiteGroupName { get; }

    /// <summary>
    /// Gets the test suite namespace.
    /// </summary>
    public string? Namespace { get; }

    /// <summary>
    /// Gets the full name of the test including namespace, test suite name and test name.
    /// </summary>
    public string? FullName { get; }

    public static bool operator ==(TestInfo left, TestInfo right) =>
        EqualityComparer<TestInfo>.Default.Equals(left, right);

    public static bool operator !=(TestInfo left, TestInfo right) =>
        !(left == right);

    private string? BuildFullName()
    {
        string[] testFullNameParts = [.. GetFullNameParts()];

        return testFullNameParts.Length > 0
            ? string.Join(".", testFullNameParts)
            : null;
    }

    private IEnumerable<string> GetFullNameParts()
    {
        if (Namespace is not null)
            yield return Namespace;

        if (SuiteName is not null)
            yield return SuiteName;

        if (Name is not null)
            yield return Name;
    }

    internal bool BelongsToNamespace(string targetNamespace)
    {
        string? thisNamespace = Namespace;
        if (thisNamespace is null || targetNamespace is null)
            return false;

        if (thisNamespace == targetNamespace)
            return true;

        return thisNamespace.StartsWith(targetNamespace, StringComparison.Ordinal) && thisNamespace[targetNamespace.Length] == '.';
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        string value = FullName ?? string.Empty;

        if (SuiteGroupName is not null)
            value += $"[{SuiteGroupName}]";

        return value;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        obj is TestInfo castedObj && Equals(castedObj);

    /// <inheritdoc/>
    public bool Equals(TestInfo other) =>
        other is not null
        && Name == other.Name
        && SuiteName == other.SuiteName
        && SuiteType == other.SuiteType
        && SuiteGroupName == other.SuiteGroupName;

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode = -316253196;
        hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(Name);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(SuiteName);
        hashCode = (hashCode * -1521134295) + EqualityComparer<Type?>.Default.GetHashCode(SuiteType);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(SuiteGroupName);
        return hashCode;
    }
}
