using System;

namespace Atata
{
    public class FindByInnerXPathAttribute : FindAttribute, ITermFindAttribute
    {
        public FindByInnerXPathAttribute(params string[] values)
        {
            values.CheckNotNullOrEmpty(nameof(values));

            Values = values;
        }

        public string[] Values { get; }

        protected override Type DefaultStrategy => typeof(FindByInnerXPathStrategy);

        public string[] GetTerms(UIComponentMetadata metadata) => Values;

        public override string BuildComponentName() =>
            BuildComponentNameWithArgument(string.Join(" or ", Values));
    }
}
