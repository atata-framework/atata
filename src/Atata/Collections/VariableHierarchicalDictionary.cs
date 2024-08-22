namespace Atata;

/// <summary>
/// Represents a variable hierarchical dictionary, which can contain a parent dictionary.
/// A variable dictionary primarily serves as a container of variables,
/// which can be used in templates via <see cref="FillTemplateString(string)"/> method
/// and similar methods for path and URI.
/// A search of elements occurs first in this dictionary and when element is not found,
/// the search continues in the parent dictionary.
/// Parent dictionary can also be a <see cref="VariableHierarchicalDictionary"/>,
/// which allows building of multi-level dictionaries.
/// </summary>
public sealed class VariableHierarchicalDictionary : HierarchicalDictionary<string, object>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VariableHierarchicalDictionary"/> class.
    /// </summary>
    /// <param name="parentDictionary">The parent dictionary, which is optional.</param>
    /// <param name="comparer">The comparer, which is optional.</param>
    public VariableHierarchicalDictionary(
        IReadOnlyDictionary<string, object> parentDictionary = null,
        IEqualityComparer<string> comparer = null)
        : base(parentDictionary, comparer)
    {
    }

    /// <summary>
    /// <para>
    /// Fills the template string with variables of this <see cref="VariableHierarchicalDictionary"/> instance.
    /// The <paramref name="template"/> can contain variables wrapped with curly braces, e.g. <c>"{varName}"</c>.
    /// </para>
    /// <para>
    /// Variables support standard .NET formatting (<c>"{numberVar:D5}"</c> or <c>"{dateTimeVar:yyyy-MM-dd}"</c>)
    /// and extended formatting for strings
    /// (for example, <c>"{stringVar:/*}"</c> appends <c>"/"</c> to the beginning of the string, if variable is not null).
    /// In order to output a <c>{</c> use <c>{{</c>, and to output a <c>}</c> use <c>}}</c>.
    /// </para>
    /// </summary>
    /// <param name="template">The template string.</param>
    /// <returns>The filled string.</returns>
    public string FillTemplateString(string template) =>
        FillTemplateString(template, null);

    /// <inheritdoc cref="FillTemplateString(string)"/>
    /// <param name="template">The template string.</param>
    /// <param name="additionalVariables">The additional variables.</param>
    public string FillTemplateString(string template, IEnumerable<KeyValuePair<string, object>> additionalVariables) =>
        TransformTemplateString(template, additionalVariables, TemplateStringTransformer.Transform);

    /// <summary>
    /// <para>
    /// Fills the path template string with variables of this <see cref="VariableHierarchicalDictionary"/> instance.
    /// The <paramref name="template"/> can contain variables wrapped with curly braces, e.g. <c>"{varName}"</c>.
    /// </para>
    /// <para>
    /// Variables are sanitized for path by replacing invalid characters with <c>'_'</c>.
    /// </para>
    /// <para>
    /// Variables support standard .NET formatting (<c>"{numberVar:D5}"</c> or <c>"{dateTimeVar:yyyy-MM-dd}"</c>)
    /// and extended formatting for strings
    /// (for example, <c>"{stringVar:/*}"</c> appends <c>"/"</c> to the beginning of the string, if variable is not null).
    /// In order to output a <c>{</c> use <c>{{</c>, and to output a <c>}</c> use <c>}}</c>.
    /// </para>
    /// </summary>
    /// <param name="template">The template string.</param>
    /// <returns>The filled string.</returns>
    public string FillPathTemplateString(string template) =>
        FillPathTemplateString(template, null);

    /// <inheritdoc cref="FillPathTemplateString(string)"/>
    /// <param name="template">The template string.</param>
    /// <param name="additionalVariables">The additional variables.</param>
    public string FillPathTemplateString(string template, IEnumerable<KeyValuePair<string, object>> additionalVariables) =>
        TransformTemplateString(template, additionalVariables, TemplateStringTransformer.TransformPath);

    /// <summary>
    /// <para>
    /// Fills the URI template string with variables of this <see cref="VariableHierarchicalDictionary"/> instance.
    /// The <paramref name="template"/> can contain variables wrapped with curly braces, e.g. <c>"{varName}"</c>.
    /// </para>
    /// <para>
    /// Variables support standard .NET formatting (<c>"{numberVar:D5}"</c> or <c>"{dateTimeVar:yyyy-MM-dd}"</c>)
    /// and extended formatting for strings
    /// (for example, <c>"{stringVar:/*}"</c> appends <c>"/"</c> to the beginning of the string, if variable is not null).
    /// In order to output a <c>{</c> use <c>{{</c>, and to output a <c>}</c> use <c>}}</c>.
    /// </para>
    /// <para>
    /// Variables are escaped by default using <see cref="Uri.EscapeDataString(string)"/> method.
    /// In order to not escape a variable, use <c>:noescape</c> modifier, for example <c>"{stringVar:noescape}"</c>.
    /// To escape a variable using <see cref="Uri.EscapeUriString(string)"/> method,
    /// preserving special URI symbols,
    /// use <c>:uriescape</c> modifier, for example <c>"{stringVar:uriescape}"</c>.
    /// Use <c>:dataescape</c> in complex scenarios (like adding optional query parameter)
    /// together with an extended formatting, for example <c>"{stringVar:dataescape:?q=*}"</c>,
    /// to escape the value and prefix it with "?q=", but nothing will be output in case
    /// <c>stringVar</c> is <see langword="null"/>.
    /// </para>
    /// </summary>
    /// <param name="template">The template string.</param>
    /// <returns>The filled string.</returns>
    public string FillUriTemplateString(string template) =>
        FillUriTemplateString(template, null);

    /// <inheritdoc cref="FillUriTemplateString(string)"/>
    /// <param name="template">The template string.</param>
    /// <param name="additionalVariables">The additional variables.</param>
    public string FillUriTemplateString(string template, IEnumerable<KeyValuePair<string, object>> additionalVariables) =>
        TransformTemplateString(template, additionalVariables, TemplateStringTransformer.TransformUri);

    private string TransformTemplateString(
        string template,
        IEnumerable<KeyValuePair<string, object>> additionalVariables,
        Func<string, IEnumerable<KeyValuePair<string, object>>, string> transformFunction)
    {
        template.CheckNotNull(nameof(template));

        if (!template.Contains('{'))
            return template;

        var allVariables = additionalVariables is null
            ? this
            : additionalVariables.Concat(this);

        return transformFunction(template, allVariables);
    }
}
