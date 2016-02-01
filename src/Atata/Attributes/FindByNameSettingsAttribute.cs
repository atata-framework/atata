namespace Atata
{
    public class FindByNameSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByNameSettingsAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(typeof(FindByNameAttribute), format, match)
        {
        }
    }
}
