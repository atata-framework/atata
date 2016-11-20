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

        public void MergeWith(ISettingsAttribute settingsAttribute)
        {
            settingsAttribute.CheckNotNull("settingsAttribute");

            if (settingsAttribute.Properties.Contains(nameof(Case)))
                Case = (TermCase)settingsAttribute.Properties[nameof(Case)];

            if (settingsAttribute.Properties.Contains(nameof(Match)))
                Match = (TermMatch)settingsAttribute.Properties[nameof(Match)];

            if (settingsAttribute.Properties.Contains(nameof(Format)))
                Format = (string)settingsAttribute.Properties[nameof(Format)];
        }
    }
}
