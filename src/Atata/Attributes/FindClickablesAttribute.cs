using System;

namespace Atata
{
    public class FindClickablesAttribute : FindControlsAttribute
    {
        public FindClickablesAttribute(FindTermBy by)
            : base(typeof(ClickableBase<>), by)
        {
        }

        public FindClickablesAttribute(Type finderType)
            : base(typeof(ClickableBase<>), finderType)
        {
        }

        public static FindAttribute CreateDefaultFindAttribute()
        {
            return new FindByContentAttribute();
        }
    }
}
