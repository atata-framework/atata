namespace Atata;

/// <summary>
/// A base attribute class for component scope definition.
/// The basic definition is represented with XPath.
/// </summary>
public abstract class ScopeDefinitionAttribute : MulticastAttribute
{
    /// <summary>
    /// The default scope XPath, which is <c>"*"</c>.
    /// </summary>
    public const string DefaultScopeXPath = "*";

    private readonly string _baseScopeXPath;

    protected ScopeDefinitionAttribute(string scopeXPath = DefaultScopeXPath) =>
        _baseScopeXPath = scopeXPath;

    /// <summary>
    /// Gets the XPath of the scope element which is a combination of XPath passed through the constructor and <see cref="ContainingClasses"/> property values.
    /// </summary>
    public string ScopeXPath => BuildScopeXPath();

    /// <summary>
    /// Gets or sets the containing CSS class name.
    /// The default value is <see langword="null"/>.
    /// </summary>
    public string? ContainingClass
    {
        get => ContainingClasses?.FirstOrDefault();
        set => ContainingClasses = value is null ? null : [value];
    }

    /// <summary>
    /// Gets or sets the containing CSS class names.
    /// Multiple class names are used in XPath as conditions joined with <c>and</c> operator.
    /// The default value is <see langword="null"/>.
    /// </summary>
    public string[]? ContainingClasses { get; set; }

    /// <summary>
    /// Builds the complete XPath of the scope element which is a combination of XPath passed through the constructor and <see cref="ContainingClasses"/> property values.
    /// </summary>
    /// <returns>The built XPath.</returns>
    protected virtual string BuildScopeXPath()
    {
        string scopeXPath = _baseScopeXPath ?? DefaultScopeXPath;

        if (ContainingClasses?.Length > 0)
        {
            var classConditions = ContainingClasses.Select(x => $"contains(concat(' ', normalize-space(@class), ' '), ' {x.Trim()} ')");
            return $"{scopeXPath}[{string.Join(" and ", classConditions)}]";
        }
        else
        {
            return scopeXPath;
        }
    }
}
