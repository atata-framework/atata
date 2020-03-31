using System;

namespace Atata
{
    /// <summary>
    /// Defines the term settings to apply for the specified finding strategy of a control.
    /// Adds to or overrides properties of <see cref="TermFindAttribute"/>.
    /// </summary>
    public class TermFindSettingsAttribute : FindSettingsAttribute, ITermSettings
    {
        public TermFindSettingsAttribute()
        {
        }

        [Obsolete("Use default constructor instead and set target attribute via TargetAttributeType property, e.g.: [TermFindSettings(TargetAttributeType = typeof(FindByNameAttribute))].")] // Obsolete since v1.0.0.
        public TermFindSettingsAttribute(FindTermBy by)
            : base(by)
        {
        }

        [Obsolete("Use default constructor instead and set target attribute via TargetAttributeType property, e.g.: [TermFindSettings(TargetAttributeType = typeof(FindByNameAttribute))].")] // Obsolete since v1.0.0.
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

        /// <summary>
        /// Gets or sets a value indicating whether the name should be cut considering the IgnoreNameEndings property value of <see cref="ControlDefinitionAttribute"/> and <see cref="PageObjectDefinitionAttribute"/>.
        /// The default value is <see langword="true"/>.
        /// </summary>
        public bool CutEnding
        {
            get { return Properties.Get(nameof(CutEnding), true); }
            set { Properties[nameof(CutEnding)] = value; }
        }
    }
}
