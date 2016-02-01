namespace Atata
{
    public class FindByContentOrValueSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByContentOrValueSettingsAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(typeof(FindByContentOrValueAttribute), format, match)
        {
        }
    }
}
