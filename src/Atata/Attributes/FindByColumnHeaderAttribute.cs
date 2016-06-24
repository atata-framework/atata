using System;

namespace Atata
{
    public class FindByColumnHeaderAttribute : TermFindAttribute
    {
        private readonly Type defaultStrategy = typeof(FindByColumnHeaderStrategy);

        public FindByColumnHeaderAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByColumnHeaderAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByColumnHeaderAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByColumnHeaderAttribute(params string[] values)
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

        // TODO: Rewiew copy/paste.
        private Type GetStrategyType(UIComponentMetadata metadata)
        {
            if (Strategy != null)
            {
                return Strategy;
            }
            else
            {
                var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByColumnHeaderSettingsAttribute>(x => x.Strategy != null);
                return settingsAttribute != null ? settingsAttribute.Strategy : defaultStrategy;
            }
        }
    }
}
