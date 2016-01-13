namespace Atata
{
    public class FindByNameSettingsAttribute : QualifierFindSettingsAttribute
    {
        public FindByNameSettingsAttribute(QualifierFormat format = QualifierFormat.Inherit, QualifierMatch match = QualifierMatch.Inherit)
            : base(format, match)
        {
        }
    }
}
