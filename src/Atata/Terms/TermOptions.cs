using System.Globalization;

namespace Atata
{
    public class TermOptions : ITermSettings
    {
        public TermOptions()
        {
            Culture = CultureInfo.CurrentCulture;
        }

        public TermCase Case { get; set; }

        public TermMatch Match { get; set; }

        public string Format { get; set; }

        public CultureInfo Culture { get; set; }

        public static TermOptions CreateDefault()
        {
            return new TermOptions();
        }

        public void MergeWith(ITermSettings otherTermSettings)
        {
            otherTermSettings.CheckNotNull("otherTermSettings");

            ISettingsAttribute settingsAttribute = otherTermSettings as ISettingsAttribute;

            if (settingsAttribute != null)
            {
                if (settingsAttribute.Properties.Contains(nameof(Case)))
                    Case = otherTermSettings.Case;

                if (settingsAttribute.Properties.Contains(nameof(Match)))
                    Match = otherTermSettings.Match;

                if (settingsAttribute.Properties.Contains(nameof(Format)))
                    Format = otherTermSettings.Format;
            }
            else
            {
                if (otherTermSettings.Case != TermCase.Inherit)
                    Case = otherTermSettings.Case;

                if (otherTermSettings.Match != TermMatch.Inherit)
                    Match = otherTermSettings.Match;

                if (otherTermSettings.Format != null)
                    Format = otherTermSettings.Format;
            }
        }
    }
}
