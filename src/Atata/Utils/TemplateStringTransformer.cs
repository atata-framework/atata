using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Provides a method for a string template transformation.
    /// </summary>
    public static class TemplateStringTransformer
    {
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

            if (template == string.Empty)
                return template;

            for (int i = 0; i < variables.Count; i++)
            {
                template = template.Replace("{" + variables.Keys.ElementAt(i), $"{{{i}");
            }

            return string.Format(ExtendedStringFormatter.Default, template, variables.Values.ToArray());
        }
    }
}
