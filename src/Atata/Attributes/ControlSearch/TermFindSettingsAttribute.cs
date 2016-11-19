using System;

namespace Atata
{
    /// <summary>
    /// Defines the term settings to apply for the specified finding strategy of a control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class TermFindSettingsAttribute : FindSettingsAttribute, ITermSettings
    {
        public TermFindSettingsAttribute(FindTermBy by)
            : base(by)
        {
        }

        public TermFindSettingsAttribute(Type findAttributeType)
            : base(findAttributeType)
        {
        }

        /// <summary>
        /// Gets or sets the term case.
        /// </summary>
        public TermCase Case
        {
            get { return Properties.Get(nameof(Case), TermCase.None); }
            set { Properties[nameof(Case)] = value; }
        }

        /// <summary>
        /// Gets or sets the match.
        /// </summary>
        public new TermMatch Match
        {
            get { return Properties.Get(nameof(Match), TermMatch.Equals); }
            set { Properties[nameof(Match)] = value; }
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format
        {
            get { return Properties.Get<string>(nameof(Format)); }
            set { Properties[nameof(Format)] = value; }
        }
    }
}
