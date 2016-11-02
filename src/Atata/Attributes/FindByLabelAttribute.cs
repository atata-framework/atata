using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the label element. Finds the &lt;label&gt; element by the specified term(s), then finds the bound control (for example, by label's "for" attribute referencing the element of the control by id). Uses <c>Title</c> as the default term case.
    /// </summary>
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
