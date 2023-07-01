namespace Atata;

/// <summary>
/// Represents the base attribute class for UI component (page object, control) definition.
/// </summary>
public abstract class UIComponentDefinitionAttribute : ScopeDefinitionAttribute
{
    protected UIComponentDefinitionAttribute(string scopeXPath = DefaultScopeXPath)
        : base(scopeXPath)
    {
    }

    /// <summary>
    /// Gets or sets the name of the component type.
    /// It is used in report log messages to describe the component type.
    /// </summary>
    public string ComponentTypeName { get; set; }

    /// <summary>
    /// Gets or sets the property name endings to ignore/truncate.
    /// Accepts a string containing a set of values separated by comma, for example <c>"Button,Link"</c>.
    /// </summary>
    public string IgnoreNameEndings { get; set; }

    /// <summary>
    /// Gets the values of property name endings to ignore.
    /// </summary>
    /// <returns>An array of name endings to ignore.</returns>
    public string[] GetIgnoreNameEndingValues() =>
        IgnoreNameEndings != null
            ? IgnoreNameEndings.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()
            : new string[0];

    /// <summary>
    /// Normalizes the name considering value of <see cref="IgnoreNameEndings"/>.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>Normalized name.</returns>
    public string NormalizeNameIgnoringEnding(string name)
    {
        string endingToIgnore = GetIgnoreNameEndingValues()
            .FirstOrDefault(x => name.EndsWith(x, StringComparison.Ordinal) && name.Length > x.Length);

        return endingToIgnore != null
            ? name.Substring(0, name.Length - endingToIgnore.Length).TrimEnd()
            : name;
    }
}
