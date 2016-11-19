using System;

namespace Atata
{
    /// <summary>
    /// Specifies the term(s) to use for the control search.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TermAttribute : TermSettingsAttribute
    {
        public TermAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public TermAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public TermAttribute(TermMatch match, params string[] values)
            : base(match)
        {
            Values = values;
        }

        public TermAttribute(params string[] values)
        {
            Values = values;
        }

        /// <summary>
        /// Gets the term values.
        /// </summary>
        public string[] Values { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the name should be cut considering the IgnoreNameEndings property value of <see cref="ControlDefinitionAttribute"/> and <see cref="PageObjectDefinitionAttribute"/>. The default value is true.
        /// </summary>
        public bool CutEnding { get; set; } = true;
    }
}
