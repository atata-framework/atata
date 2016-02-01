namespace Atata
{
    public class FindByClassAttribute : TermFindAttribute
    {
        public FindByClassAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByClassAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByClassAttribute(string value, TermMatch match)
            : base(value, match)
        {
        }

        public FindByClassAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Dashed; }
        }

        protected override TermMatch DefaultMatch
        {
            get { return TermMatch.Equals; }
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

        protected override TermMatch GetTermMatchFromMetadata(UIComponentMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByClassSettingsAttribute>(x => x.Match != TermMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
