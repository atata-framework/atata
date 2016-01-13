namespace Atata
{
    public class FindByNameAttribute : QualifierMatchFindAttribute
    {
        private const QualifierFormat DefaultFormat = QualifierFormat.Dashed;
        private const QualifierMatch DefaultMatch = QualifierMatch.Equals;

        public FindByNameAttribute(QualifierFormat format)
            : base(format)
        {
        }

        public FindByNameAttribute(params string[] values)
            : base(values)
        {
        }

        public override IElementFindStrategy CreateStrategy(UIPropertyMetadata metadata)
        {
            return new FindByNameStrategy();
        }

        protected override QualifierFormat GetQualifierFormatFromMetadata(UIPropertyMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByNameSettingsAttribute>(x => x.Format != QualifierFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }

        protected override QualifierMatch GetQualifierMatchFromMetadata(UIPropertyMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByNameSettingsAttribute>(x => x.Match != QualifierMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
