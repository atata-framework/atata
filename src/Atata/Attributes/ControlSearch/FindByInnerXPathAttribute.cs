using System;

namespace Atata
{
    public class FindByInnerXPathAttribute : FindAttribute, ITermFindAttribute
    {
        public FindByInnerXPathAttribute(params string[] values)
        {
            Values = values;
        }

        public string[] Values { get; }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByInnerXPathStrategy); }
        }

        public string[] GetTerms(UIComponentMetadata metadata)
        {
            return Values;
        }
    }
}
