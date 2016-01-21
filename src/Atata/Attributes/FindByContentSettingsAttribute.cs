namespace Atata
{
    public class FindByContentSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByContentSettingsAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }
    }
}
