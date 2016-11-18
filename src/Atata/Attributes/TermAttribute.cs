using System;

namespace Atata
{
    /// <summary>
    /// Specifies the term(s) to use for the control search.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TermAttribute : Attribute, ITermSettings
    {
        public TermAttribute(TermCase termCase)
            : this(null, termCase: termCase)
        {
        }

        public TermAttribute(TermMatch match, TermCase termCase)
            : this(null, match, termCase)
        {
        }

        public TermAttribute(TermMatch match, params string[] values)
            : this(values, match)
        {
        }

        public TermAttribute(params string[] values)
            : this(values, TermMatch.Inherit)
        {
        }

        private TermAttribute(string[] values = null, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
        {
            Values = values;
            Match = match;
            Case = termCase;
        }

        /// <summary>
        /// Gets the term values.
        /// </summary>
        public string[] Values { get; private set; }

        /// <summary>
        /// Gets the match.
        /// </summary>
        public new TermMatch Match { get; private set; }

        /// <summary>
        /// Gets the term case.
        /// </summary>
        public TermCase Case { get; private set; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the name should be cut considering the IgnoreNameEndings property value of <see cref="ControlDefinitionAttribute"/> and <see cref="PageObjectDefinitionAttribute"/>. The default value is true.
        /// </summary>
        public bool CutEnding { get; set; } = true;
    }
}
