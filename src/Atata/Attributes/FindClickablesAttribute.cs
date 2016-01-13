using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    public class FindClickablesAttribute : Attribute
    {
        public FindClickablesAttribute(FindClickableBy by)
        {
            By = by;
        }

        public FindClickableBy By { get; private set; }

        public FindAttribute CreateFindAttribute()
        {
            // TODO: Finish this switch.
            switch (By)
            {
                case FindClickableBy.Id:
                    return new FindByIdAttribute();
                case FindClickableBy.Content:
                    return new FindByContentAttribute();
                default:
                    throw new InvalidOperationException("Unknown 'By' value.");
            }
        }

        public static FindAttribute CreateDefaultFindAttribute()
        {
            return new FindByContentAttribute();
        }
    }
}
