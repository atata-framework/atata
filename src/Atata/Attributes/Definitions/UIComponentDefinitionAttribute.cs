namespace Atata;

/// <summary>
/// A base attribute class for UI component (page object, control) definition.
/// </summary>
public abstract class UIComponentDefinitionAttribute : ScopeDefinitionAttribute
{
    private static readonly char[] s_ignoreNameEndingValueSeparator = [','];

    protected UIComponentDefinitionAttribute(string scopeXPath = DefaultScopeXPath)
        : base(scopeXPath)
    {
    }

    /// <summary>
    /// Gets or sets the name of the component type.
    /// It is used in report log messages to describe the component type.
    /// The default value is <see langword="null"/>,
    /// meaning that the component type name will be resolved automatically.
    /// </summary>
    public string? ComponentTypeName { get; set; }

    /// <summary>
    /// Gets or sets the property name endings to ignore/truncate.
    /// Accepts a string containing a set of values separated by comma, for example <c>"Button,Link"</c>.
    /// The default value is <see langword="null"/>.
    /// </summary>
    public string? IgnoreNameEndings { get; set; }

    /// <summary>
    /// Gets the values of property name endings to ignore.
    /// </summary>
    /// <returns>An array of name endings to ignore.</returns>
    public string[] GetIgnoreNameEndingValues() =>
        IgnoreNameEndings is not null
            ? IgnoreNameEndings.Split(s_ignoreNameEndingValueSeparator, StringSplitOptions.RemoveEmptyEntries)
            : [];

    /// <summary>
    /// Normalizes the name considering value of <see cref="IgnoreNameEndings"/>.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>Normalized name.</returns>
    public string NormalizeNameIgnoringEnding(string name)
    {
        string? endingToIgnore = GetIgnoreNameEndingValues()
            .FirstOrDefault(x => name.EndsWith(x, StringComparison.Ordinal) && name.Length > x.Length);

        return endingToIgnore is not null
            ? name[..^endingToIgnore.Length].TrimEnd()
            : name;
    }
}
