namespace Atata
{
    public static class ITermSettingsExtensions
    {
        public static TermCase? GetCaseOrNull(this ITermSettings termSettings)
        {
            return termSettings != null && termSettings.Case != TermCase.Inherit
                ? termSettings.Case
                : (TermCase?)null;
        }

        public static TermMatch? GetMatchOrNull(this ITermSettings termSettings)
        {
            return termSettings != null && termSettings.Match != TermMatch.Inherit
                ? termSettings.Match
                : (TermMatch?)null;
        }

        public static string GetFormatOrNull(this ITermSettings termSettings)
        {
            return termSettings != null && termSettings.Format != null
                ? termSettings.Format
                : null;
        }
    }
}
