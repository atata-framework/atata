using System.Globalization;

namespace Atata
{
    public class TermOptions : ITermSettings, IPropertySettings
    {
        public PropertyBag Properties { get; } = new PropertyBag();

        /// <summary>
        /// Gets or sets the match.
        /// </summary>
        public TermMatch Match
        {
            get { return Properties.Get(nameof(Match), TermMatch.Equals); }
            set { Properties[nameof(Match)] = value; }
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
        /// Gets or sets the format.
        /// </summary>
        public string Format
        {
            get { return Properties.Get<string>(nameof(Format)); }
            set { Properties[nameof(Format)] = value; }
        }

        public CultureInfo Culture
        {
            get { return Properties.Get(nameof(Culture), CultureInfo.CurrentCulture); }
            set { Properties[nameof(Culture)] = value; }
        }

        public void MergeWith(IPropertySettings settingsAttribute)
        {
            settingsAttribute.CheckNotNull(nameof(settingsAttribute));

            if (settingsAttribute.Properties.Contains(nameof(Case)))
                Case = (TermCase)settingsAttribute.Properties[nameof(Case)];

            if (settingsAttribute.Properties.Contains(nameof(Match)))
                Match = (TermMatch)settingsAttribute.Properties[nameof(Match)];

            if (settingsAttribute.Properties.Contains(nameof(Format)))
                Format = (string)settingsAttribute.Properties[nameof(Format)];
        }
    }
}
