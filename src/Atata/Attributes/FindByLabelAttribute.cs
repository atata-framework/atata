using System;

namespace Atata
{
    public class FindByLabelAttribute : TermFindAttribute
    {
        private readonly Type defaultStrategy = typeof(FindByLabelStrategy);

        public FindByLabelAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByLabelAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public FindByLabelAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByLabelAttribute(params string[] values)
            : base(values)
        {
        }

        public Type Strategy { get; set; }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
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
