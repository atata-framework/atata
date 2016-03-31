namespace Atata
{
    public static class ITermSettingsExtensions
    {
        public static TermFormat? GetFormatOrNull(this ITermSettings termSettings)
        {
            return termSettings != null && termSettings.Format != TermFormat.Inherit
                ? termSettings.Format
                : (TermFormat?)null;
        }

        public static TermMatch? GetMatchOrNull(this ITermSettings termSettings)
        {
            return termSettings != null && termSettings.Match != TermMatch.Inherit
                ? termSettings.Match
                : (TermMatch?)null;
        }

        public static string GetStringFormatOrNull(this ITermSettings termSettings)
        {
            return termSettings != null && termSettings.StringFormat != null
                ? termSettings.StringFormat
                : null;
        }
    }
}
