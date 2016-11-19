using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FindItemByValueAttribute : TermSettingsAttribute, IFindItemAttribute
    {
        public FindItemByValueAttribute()
        {
        }

        public FindItemByValueAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindItemByValueAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindItemByValueAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByValueStrategy();
        }
    }
}
