using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public class FindItemByLabelAttribute : TermSettingsAttribute, IFindItemAttribute
    {
        public FindItemByLabelAttribute()
        {
        }

        public FindItemByLabelAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindItemByLabelAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindItemByLabelAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public IItemElementFindStrategy CreateStrategy(UIComponent component, UIComponentMetadata metadata)
        {
            return new FindItemByLabelStrategy(component);
        }
    }
}
