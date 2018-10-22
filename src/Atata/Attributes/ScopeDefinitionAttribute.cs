using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for component scope definition.
    /// The basic definition is represented with XPath.
    /// </summary>
    public abstract class ScopeDefinitionAttribute : MulticastAttribute
    {
        /// <summary>
        /// The default scope XPath, which is <c>"*"</c>.
        /// </summary>
        public const string DefaultScopeXPath = "*";

        private readonly string baseScopeXPath;

        protected ScopeDefinitionAttribute(string scopeXPath = DefaultScopeXPath)
        {
            baseScopeXPath = scopeXPath;
        }

        /// <summary>
        /// Gets the XPath of the scope element which is a combination of XPath passed through the constructor and <see cref="ContainingClasses"/> property values.
        /// </summary>
        public string ScopeXPath => BuildScopeXPath();

        /// <summary>
        /// Gets or sets the containing CSS class name.
        /// </summary>
        public string ContainingClass
        {
            get => ContainingClasses?.SingleOrDefault();
            set => ContainingClasses = value == null ? null : new[] { value };
        }

        /// <summary>
        /// Gets or sets the containing CSS class names.
        /// Multiple class names are used in XPath as conditions with <c>and</c> operator.
        /// </summary>
        public string[] ContainingClasses { get; set; }

        /// <summary>
        /// Builds the complete XPath of the scope element which is a combination of XPath passed through the constructor and <see cref="ContainingClasses"/> property values.
        /// </summary>
        /// <returns>The built XPath.</returns>
        protected virtual string BuildScopeXPath()
        {
            string scopeXPath = baseScopeXPath ?? DefaultScopeXPath;

            if (ContainingClasses?.Any() ?? false)
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
}
