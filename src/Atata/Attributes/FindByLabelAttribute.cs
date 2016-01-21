using System;

namespace Atata
{
    public class FindByLabelAttribute : TermFindAttribute
    {
        private const TermFormat DefaultFormat = TermFormat.Title;
        private const TermMatch DefaultMatch = TermMatch.Equals;

        private readonly Type defaultStrategy = typeof(FindByLabelStrategy);

        public FindByLabelAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByLabelAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByLabelAttribute(string value, TermMatch match)
            : base(value, match)
        {
        }

        public FindByLabelAttribute(params string[] values)
            : base(values)
        {
        }

        public Type Strategy { get; set; }

        public override IElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            Type strategyType = GetStrategyType(metadata);
            return (IElementFindStrategy)Activator.CreateInstance(strategyType);
        }

        private Type GetStrategyType(UIComponentMetadata metadata)
        {
            if (Strategy != null)
            {
                return Strategy;
            }
            else
            {
                var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByLabelSettingsAttribute>(x => x.Strategy != null);
                return settingsAttribute != null ? settingsAttribute.Strategy : defaultStrategy;
            }
        }

        protected override TermFormat GetTermFormatFromMetadata(UIComponentMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByLabelSettingsAttribute>(x => x.Format != TermFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }

        protected override TermMatch GetTermMatchFromMetadata(UIComponentMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByLabelSettingsAttribute>(x => x.Match != TermMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
