namespace Atata;

/// <summary>
/// Provides a set of methods for a string template transformation.
/// </summary>
public static class TemplateStringTransformer
{
    private static readonly Regex s_variableMatchRegex = new(@"{\D[^}]*}");

    private static readonly char[] s_braces = new[] { '{', '}' };

    /// <summary>
    /// Determines whether the template has any variable and can be transformed.
    /// </summary>
    /// <param name="template">The template.</param>
    /// <returns>
    /// <see langword="true"/> if the template can be transformed; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool CanTransform(string template) =>
        template != null && template.IndexOfAny(s_braces) >= 0;

    /// <summary>
    /// <para>
    /// Transforms the specified template by filling it with variables.
    /// The <paramref name="template"/> can contain variables wrapped with curly braces, e.g. <c>"{varName}"</c>.
    /// </para>
    /// <para>
    /// Variables support standard .NET formatting (<c>"{numberVar:D5}"</c> or <c>"{dateTimeVar:yyyy-MM-dd}"</c>)
    /// and extended formatting for strings
    /// (for example, <c>"{stringVar:/*}"</c> appends <c>"/"</c> to the beginning of the string, if variable is not null).
    /// In order to output a <c>{</c> use <c>{{</c>, and to output a <c>}</c> use <c>}}</c>.
    /// </para>
    /// </summary>
    /// <param name="template">The template.</param>
    /// <param name="variables">The variables.</param>
    /// <returns>The string result.</returns>
    public static string Transform(string template, IDictionary<string, object> variables) =>
        Transform(template, variables, AtataTemplateStringFormatter.Default);

    /// <summary>
    /// <para>
    /// Transforms the specified path template by filling it with variables.
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
    /// <param name="template">The template.</param>
    /// <param name="variables">The variables.</param>
    /// <returns>The string result.</returns>
    public static string TransformPath(string template, IDictionary<string, object> variables) =>
        Transform(template, variables, AtataPathTemplateStringFormatter.Default);

    /// <summary>
    /// <para>
    /// Transforms the specified URI template by filling it with variables.
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
    /// <param name="template">The template.</param>
    /// <param name="variables">The variables.</param>
    /// <returns>The string result.</returns>
    public static string TransformUri(string template, IDictionary<string, object> variables) =>
        Transform(template, variables, AtataUriTemplateStringFormatter.Default);

    private static string Transform(string template, IDictionary<string, object> variables, IFormatProvider formatProvider)
    {
        template.CheckNotNull(nameof(template));
        variables.CheckNotNull(nameof(variables));

        if (CanTransform(template))
        {
            string workingTemplate = template;
            var variablesKeys = variables.OrderByDescending(x => x.Key.Length)
                .Select(x => x.Key)
                .ToArray();
            var variablesValues = variables.OrderByDescending(x => x.Key.Length)
                .Select(x => x.Value)
                .ToArray();

            for (int i = 0; i < variablesKeys.Length; i++)
            {
                workingTemplate = workingTemplate.Replace("{" + variablesKeys[i], $"{{{i}");
            }

            try
            {
                return string.Format(formatProvider, workingTemplate, variablesValues);
            }
            catch (FormatException)
            {
                MatchCollection matches = s_variableMatchRegex.Matches(workingTemplate);

                if (matches.Count > 0)
                {
                    string unknownVariablesString = string.Join(", ", matches.OfType<Match>().Select(x => x.Value).Distinct());
                    throw new FormatException($"Template \"{template}\" string contains unknown variable(s): {unknownVariablesString}.");
                }
                else
                {
                    throw new FormatException($"Template \"{template}\" string is not in a correct format.");
                }
            }
        }
        else
        {
            return template;
        }
    }
}
