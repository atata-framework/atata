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
        public string StringFormat { get; set; }
        public CultureInfo Culture { get; set; }

        public static TermOptions CreateDefault()
        {
            return new TermOptions();
        }

        public void MergeWith(ITermSettings otherTermSettings)
        {
            otherTermSettings.CheckNotNull("otherTermSettings");

            if (otherTermSettings.Case != TermCase.Inherit)
                Case = otherTermSettings.Case;

            if (otherTermSettings.Match != TermMatch.Inherit)
                Match = otherTermSettings.Match;

            if (otherTermSettings.StringFormat != null)
                StringFormat = otherTermSettings.StringFormat;
        }
    }
}
