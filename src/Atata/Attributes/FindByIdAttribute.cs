namespace Atata
{
    public class FindByIdAttribute : TermFindAttribute
    {
        private const TermFormat DefaultFormat = TermFormat.Dashed;
        private const TermMatch DefaultMatch = TermMatch.Equals;

        public FindByIdAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByIdAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByIdAttribute(string value, TermMatch match)
            : base(value, match)
        {
        }

        public FindByIdAttribute(params string[] values)
            : base(values)
        {
        }

        public override IElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByIdStrategy();
        }

        protected override TermFormat GetTermFormatFromMetadata(UIComponentMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByIdSettingsAttribute>(x => x.Format != TermFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }

        protected override TermMatch GetTermMatchFromMetadata(UIComponentMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByIdSettingsAttribute>(x => x.Match != TermMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
