namespace Atata
{
    public class FindByIdSettingsAttribute : QualifierFindSettingsAttribute
    {
        public FindByIdSettingsAttribute(QualifierFormat format = QualifierFormat.Inherit, QualifierMatch match = QualifierMatch.Inherit)
            : base(format, match)
        {
        }
    }
}
