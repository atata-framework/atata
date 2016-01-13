namespace Atata
{
    public class FindByContentAttribute : QualifierMatchFindAttribute
    {
        private const QualifierFormat DefaultFormat = QualifierFormat.Title;
        private const QualifierMatch DefaultMatch = QualifierMatch.Equals;

        public FindByContentAttribute(QualifierFormat format)
            : base(format)
        {
        }

        public FindByContentAttribute(params string[] values)
            : base(values)
        {
        }

        public override IElementFindStrategy CreateStrategy(UIPropertyMetadata metadata)
        {
            return new FindByContentStrategy();
        }

        protected override QualifierFormat GetQualifierFormatFromMetadata(UIPropertyMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByContentSettingsAttribute>(x => x.Format != QualifierFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }

        protected override QualifierMatch GetQualifierMatchFromMetadata(UIPropertyMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByContentSettingsAttribute>(x => x.Match != QualifierMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
