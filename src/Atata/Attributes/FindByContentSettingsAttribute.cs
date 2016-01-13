namespace Atata
{
    public class FindByContentSettingsAttribute : QualifierFindSettingsAttribute
    {
        public FindByContentSettingsAttribute(QualifierFormat format = QualifierFormat.Inherit, QualifierMatch match = QualifierMatch.Inherit)
            : base(format, match)
        {
        }
    }
}
