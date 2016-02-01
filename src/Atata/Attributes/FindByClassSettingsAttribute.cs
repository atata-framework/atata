namespace Atata
{
    public class FindByClassSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByClassSettingsAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(typeof(FindByClassAttribute), format, match)
        {
        }
    }
}
