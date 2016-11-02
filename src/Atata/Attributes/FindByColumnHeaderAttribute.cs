using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found within the table column (&lt;td&gt;) that has the header (&lt;th&gt;) matching the specified term(s). Uses <c>Title</c> as the default term case.
    /// </summary>
    public class FindByColumnHeaderAttribute : TermFindAttribute
    {
        private readonly Type defaultStrategy = typeof(FindByColumnHeaderStrategy);

        public FindByColumnHeaderAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByColumnHeaderAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
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

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
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
                var settingsAttribute = metadata.GetFirstOrDefaultAttribute<FindByColumnHeaderSettingsAttribute>(x => x.Strategy != null);
                return settingsAttribute != null ? settingsAttribute.Strategy : defaultStrategy;
            }
        }
    }
}
