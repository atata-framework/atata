using System;

namespace Atata
{
    public class FindByLabelAttribute : TermFindAttribute
    {
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

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            Type strategyType = GetStrategyType(metadata);
            return (IComponentScopeLocateStrategy)ActivatorEx.CreateInstance(strategyType);
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
    }
}
