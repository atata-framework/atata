namespace Atata
{
    public class FindByValueSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByValueSettingsAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(typeof(FindByValueAttribute), format, match)
        {
        }
    }
}
