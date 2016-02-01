namespace Atata
{
    public class FindByNameAttribute : TermFindAttribute
    {
        public FindByNameAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByNameAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByNameAttribute(string value, TermMatch match)
            : base(value, match)
        {
        }

        public FindByNameAttribute(params string[] values)
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
            return new FindByNameStrategy();
        }

        protected override TermFormat GetTermFormatFromMetadata(UIComponentMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByNameSettingsAttribute>(x => x.Format != TermFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }

        protected override TermMatch GetTermMatchFromMetadata(UIComponentMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByNameSettingsAttribute>(x => x.Match != TermMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
