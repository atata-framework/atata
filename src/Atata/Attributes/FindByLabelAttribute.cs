using System;

namespace Atata
{
    public class FindByLabelAttribute : QualifierMatchFindAttribute
    {
        private const QualifierFormat DefaultFormat = QualifierFormat.Title;
        private const QualifierMatch DefaultMatch = QualifierMatch.Equals;

        private readonly Type defaultStrategy = typeof(FindByLabelStrategy);

        public FindByLabelAttribute(QualifierFormat format = QualifierFormat.Inherit, QualifierMatch match = QualifierMatch.Inherit)
            : base(format, match)
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

        protected override QualifierFormat GetQualifierFormatFromMetadata(UIComponentMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByLabelSettingsAttribute>(x => x.Format != QualifierFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }

        protected override QualifierMatch GetQualifierMatchFromMetadata(UIComponentMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByLabelSettingsAttribute>(x => x.Match != QualifierMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
