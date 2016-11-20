namespace Atata
{
    // TODO: Review and clean ITermSettingsExtensions.
    public static class ITermSettingsExtensions
    {
        public static TermCase? GetCaseOrNull(this ITermSettings termSettings)
        {
            if (termSettings == null)
                return null;

            IPropertySettings castedTermSettings = termSettings as IPropertySettings;

            if (castedTermSettings != null)
                return castedTermSettings.Properties.Contains(nameof(ITermSettings.Case)) ? termSettings.Case : (TermCase?)null;
            else
                return termSettings.Case;
        }

        public static TermMatch? GetMatchOrNull(this ITermSettings termSettings)
        {
            if (termSettings == null)
                return null;

            IPropertySettings castedTermSettings = termSettings as IPropertySettings;

            if (castedTermSettings != null)
                return castedTermSettings.Properties.Contains(nameof(ITermSettings.Match)) ? termSettings.Match : (TermMatch?)null;
            else
                return termSettings.Match;
        }

        public static string GetFormatOrNull(this ITermSettings termSettings)
        {
            return termSettings?.Format;
        }
    }
}
