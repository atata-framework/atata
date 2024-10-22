namespace Atata;

/// <summary>
/// Represents the basic test information.
/// </summary>
public sealed class TestInfo
{
    public TestInfo(string name, string suiteName, Type suiteType)
    {
        Name = name;
        NameSanitized = name?.SanitizeForFileName(AtataPathTemplateStringFormatter.CharToReplaceWith);

        SuiteName = suiteName ?? suiteType?.Name;
        SuiteNameSanitized = SuiteName?.SanitizeForFileName(AtataPathTemplateStringFormatter.CharToReplaceWith);

        SuiteType = suiteType;
        FullName = BuildFullName();
    }

    /// <summary>
    /// Gets the name of the test.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the name of the test sanitized for file path/name.
    /// </summary>
    public string NameSanitized { get; }

    /// <summary>
    /// Gets the name of the test suite (fixture/class).
    /// </summary>
    public string SuiteName { get; }

    /// <summary>
    /// Gets the name of the test suite (fixture/class) sanitized for file path/name.
    /// </summary>
    public string SuiteNameSanitized { get; }

    /// <summary>
    /// Gets the test suite (fixture/class) type.
    /// </summary>
    public Type SuiteType { get; }

    /// <summary>
    /// Gets the full name of the test including namespace, test suite name and test name.
    /// </summary>
    public string FullName { get; }

    private string BuildFullName()
    {
        string[] testFullNameParts = GetFullNameParts().ToArray();

        return testFullNameParts.Length > 0
            ? string.Join(".", testFullNameParts)
            : null;
    }

    private IEnumerable<string> GetFullNameParts()
    {
        if (SuiteType != null)
            yield return SuiteType.Namespace;

        if (SuiteName != null)
            yield return SuiteName;

        if (Name != null)
            yield return Name;
    }

    internal string GetTestUnitKindName() =>
        Name != null
            ? "test"
            : SuiteType != null
                ? "test suite"
                : "test unit";

    /// <inheritdoc/>
    public override string ToString() =>
        FullName ?? string.Empty;
}
