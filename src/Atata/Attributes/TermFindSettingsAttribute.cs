using System;

namespace Atata
{
    /// <summary>
    /// Defines the term settings to apply for the specified finding strategy of a control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class TermFindSettingsAttribute : FindSettingsAttribute, ITermSettings
    {
        public TermFindSettingsAttribute(FindTermBy by, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : this(by.ResolveFindAttributeType(), match, termCase)
        {
        }

        public TermFindSettingsAttribute(Type findAttributeType, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : base(findAttributeType)
        {
            Match = match;
            Case = termCase;
        }

        /// <summary>
        /// Gets or sets the term case.
        /// </summary>
        public TermCase Case { get; set; }

        /// <summary>
        /// Gets or sets the match.
        /// </summary>
        public new TermMatch Match { get; set; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format { get; set; }
    }
}
