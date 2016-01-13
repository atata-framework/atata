namespace Atata
{
    public class FindByClassAttribute : QualifierFindAttribute
    {
        private const QualifierFormat DefaultFormat = QualifierFormat.Dashed;
        private const QualifierMatch DefaultMatch = QualifierMatch.Contains;

        public FindByClassAttribute(QualifierFormat format)
            : base(format)
        {
        }

        public FindByClassAttribute(params string[] values)
            : base(values)
        {
        }

        public override IElementFindStrategy CreateStrategy(UIPropertyMetadata metadata)
        {
            return new FindByClassStrategy();
        }

        protected override QualifierFormat GetQualifierFormatFromMetadata(UIPropertyMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByClassSettingsAttribute>(x => x.Format != QualifierFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }
    }
}
