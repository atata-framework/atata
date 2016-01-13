namespace Atata
{
    public class FindByClassSettingsAttribute : QualifierFindSettingsAttribute
    {
        public FindByClassSettingsAttribute(QualifierFormat format = QualifierFormat.Inherit, QualifierMatch match = QualifierMatch.Inherit)
            : base(format, match)
        {
        }
    }
}
