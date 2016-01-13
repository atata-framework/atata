namespace Atata
{
    public class FindByIdAttribute : QualifierMatchFindAttribute
    {
        private const QualifierFormat DefaultFormat = QualifierFormat.Dashed;
        private const QualifierMatch DefaultMatch = QualifierMatch.Equals;

        public FindByIdAttribute(QualifierFormat format)
            : base(format)
        {
        }

        public FindByIdAttribute(params string[] values)
            : base(values)
        {
        }

        public override IElementFindStrategy CreateStrategy(UIPropertyMetadata metadata)
        {
            return new FindByIdStrategy();
        }

        protected override QualifierFormat GetQualifierFormatFromMetadata(UIPropertyMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByIdSettingsAttribute>(x => x.Format != QualifierFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }

        protected override QualifierMatch GetQualifierMatchFromMetadata(UIPropertyMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByIdSettingsAttribute>(x => x.Match != QualifierMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
