namespace Atata
{
    // TODO: Review and clean ITermSettingsExtensions.
    public static class ITermSettingsExtensions
    {
        public static TermCase? GetCaseOrNull(this ITermSettings termSettings)
        {
            if (termSettings == null)
                return null;

            ISettingsAttribute termSettingsAsAttribute = termSettings as ISettingsAttribute;

            if (termSettingsAsAttribute != null)
                return termSettingsAsAttribute.Properties.Contains("Case") ? termSettings.Case : (TermCase?)null;
            else
                return termSettings.Case != TermCase.Inherit ? termSettings.Case : (TermCase?)null;
        }

        public static TermMatch? GetMatchOrNull(this ITermSettings termSettings)
        {
            if (termSettings == null)
                return null;

            ISettingsAttribute termSettingsAsAttribute = termSettings as ISettingsAttribute;

            if (termSettingsAsAttribute != null)
                return termSettingsAsAttribute.Properties.Contains("Match") ? termSettings.Match : (TermMatch?)null;
            else
                return termSettings.Match != TermMatch.Inherit ? termSettings.Match : (TermMatch?)null;
        }

        public static string GetFormatOrNull(this ITermSettings termSettings)
        {
            return termSettings?.Format;
        }
    }
}
