using System;

namespace Atata
{
    public class FindByColumnAttribute : TermFindAttribute
    {
        private readonly bool useIndexStrategy;
        private readonly Type defaultStrategy = typeof(FindByColumnHeaderStrategy);

        public FindByColumnAttribute(int columnIndex)
            : base()
        {
            ColumnIndex = columnIndex;
            useIndexStrategy = true;
        }

        public FindByColumnAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByColumnAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByColumnAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByColumnAttribute(params string[] values)
            : base(values)
        {
        }

        public int ColumnIndex { get; private set; }
        public Type Strategy { get; set; }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            if (useIndexStrategy)
            {
                return new FindByColumnIndexStrategy(ColumnIndex);
            }
            else
            {
                Type strategyType = GetStrategyType(metadata);
                return (IComponentScopeLocateStrategy)ActivatorEx.CreateInstance(strategyType);
            }
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
                var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindByColumnSettingsAttribute>(x => x.Strategy != null);
                return settingsAttribute != null ? settingsAttribute.Strategy : defaultStrategy;
            }
        }
    }
}
