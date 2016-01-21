namespace Atata
{
    public class FindByIdSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByIdSettingsAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }
    }
}
