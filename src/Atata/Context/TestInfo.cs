namespace Atata;

public sealed class TestInfo
{
    private string _name;

    private string _suiteName;

    /// <summary>
    /// Gets the name of the test.
    /// </summary>
    public string Name
    {
        get => _name;
        internal set
        {
            _name = value;
            NameSanitized = value.SanitizeForFileName(AtataPathTemplateStringFormatter.CharToReplaceWith);
        }
    }

    /// <summary>
    /// Gets the name of the test sanitized for file path/name.
    /// </summary>
    public string NameSanitized { get; private set; }

    /// <summary>
    /// Gets the name of the test suite (fixture/class).
    /// </summary>
    public string SuiteName
    {
        get => _suiteName;
        internal set
        {
            _suiteName = value;
            SuiteNameSanitized = value.SanitizeForFileName(AtataPathTemplateStringFormatter.CharToReplaceWith);
        }
    }

    /// <summary>
    /// Gets the name of the test suite sanitized for file path/name.
    /// </summary>
    public string SuiteNameSanitized { get; private set; }

    /// <summary>
    /// Gets the test suite (fixture/class) type.
    /// </summary>
    public Type SuiteType { get; internal set; }

    /// <summary>
    /// Gets the full name of the test including namespace, test suite name and test name.
    /// </summary>
    public string FullName
    {
        get
        {
            string[] testFullNameParts = GetFullNameParts().ToArray();

            return testFullNameParts.Length > 0
                ? string.Join(".", testFullNameParts)
                : null;
        }
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
}
