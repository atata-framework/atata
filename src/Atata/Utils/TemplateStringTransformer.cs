using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Atata
{
    /// <summary>
    /// Provides a set of methods for a string template transformation.
    /// </summary>
    public static class TemplateStringTransformer
    {
        private static readonly Regex s_variableMatchRegex = new Regex(@"{\D[^}]*}");

        /// <summary>
        /// Determines whether the template has any variable and can be transformed.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns>
        /// <c>true</c> if the template can be transformed; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanTransform(string template) =>
            template?.Contains('{') ?? false;

        /// <summary>
        /// Transforms the specified template by filling it with variables.
        /// The <paramref name="template"/> can contain variables wrapped with curly braces, e.g. <c>"{varName}"</c>.
        /// Variables support standard .NET formatting (<c>"{numberVar:D5}"</c> or <c>"{dateTimeVar:yyyy-MM-dd}"</c>)
        /// and extended formatting for strings
        /// (for example, <c>"{stringVar:/*}"</c> appends <c>"/"</c> to the beginning of the string, if variable is not null).
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="variables">The variables.</param>
        /// <returns>The string result.</returns>
        public static string Transform(string template, IDictionary<string, object> variables)
        {
            template.CheckNotNull(nameof(template));
            variables.CheckNotNull(nameof(variables));

            if (CanTransform(template))
            {
                string workingTemplate = template;

                for (int i = 0; i < variables.Count; i++)
                {
                    workingTemplate = workingTemplate.Replace("{" + variables.Keys.ElementAt(i), $"{{{i}");
                }

                try
                {
                    return string.Format(ExtendedStringFormatter.Default, workingTemplate, variables.Values.ToArray());
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
}
