using System.Globalization;

namespace Atata
{
    public class TermOptions : ITermSettings
    {
        public TermFormat Format { get; set; }
        public TermMatch Match { get; set; }
        public string StringFormat { get; set; }
        public CultureInfo Culture { get; set; }

        public static TermOptions CreateDefault()
        {
            return new TermOptions
            {
                Format = TermFormat.Inherit,
                Match = TermMatch.Inherit,
                StringFormat = null,
                Culture = CultureInfo.CurrentCulture
            };
        }
    }
}
