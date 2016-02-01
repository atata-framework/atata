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

        public FindByColumnAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByColumnAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByColumnAttribute(string value, TermMatch match)
            : base(value, match)
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

        protected override TermMatch DefaultMatch
        {
            get { return TermMatch.Equals; }
        }

        public override IElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
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
