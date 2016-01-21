namespace Atata
{
    public class FindByClassAttribute : TermFindAttribute
    {
        private const TermFormat DefaultFormat = TermFormat.Dashed;
        private const TermMatch DefaultMatch = TermMatch.Contains;

        public FindByClassAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByClassAttribute(params string[] values)
            : base(values)
        {
        }

        public override IElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByClassStrategy();
        }

        protected override TermFormat GetTermFormatFromMetadata(UIComponentMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByClassSettingsAttribute>(x => x.Format != TermFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }
    }
}
