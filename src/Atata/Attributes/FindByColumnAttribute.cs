using System;

namespace Atata
{
    public class FindByColumnAttribute : QualifierMatchFindAttribute
    {
        private const QualifierFormat DefaultFormat = QualifierFormat.Title;
        private const QualifierMatch DefaultMatch = QualifierMatch.Equals;

        private readonly Type defaultStrategy = typeof(FindByColumnHeaderStrategy);

        private bool useIndexStrategy;

        public FindByColumnAttribute()
        {
        }

        public FindByColumnAttribute(int columnIndex)
        {
            ColumnIndex = columnIndex;
            useIndexStrategy = true;
        }

        public FindByColumnAttribute(params string[] values)
            : base(values)
        {
        }

        public int ColumnIndex { get; private set; }
        public Type Strategy { get; set; }

        public override IElementFindStrategy CreateStrategy(UIPropertyMetadata metadata)
        {
            if (useIndexStrategy)
            {
                return new FindByColumnIndexStrategy(ColumnIndex);
            }
            else
            {
                Type strategyType = GetStrategyType(metadata);
                return (IElementFindStrategy)Activator.CreateInstance(strategyType);
            }
        }

        // TODO: Rewiew copy/paste.
        private Type GetStrategyType(UIPropertyMetadata metadata)
        {
            if (Strategy != null)
            {
                return Strategy;
            }
            else
            {
                var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByColumnSettingsAttribute>(x => x.Strategy != null);
                return settingsAttribute != null ? settingsAttribute.Strategy : defaultStrategy;
            }
        }

        protected override QualifierFormat GetQualifierFormatFromMetadata(UIPropertyMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByColumnSettingsAttribute>(x => x.Format != QualifierFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }

        protected override QualifierMatch GetQualifierMatchFromMetadata(UIPropertyMetadata metadata)
        {
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByColumnSettingsAttribute>(x => x.Match != QualifierMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
