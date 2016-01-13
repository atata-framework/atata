using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    public class FindEditableFieldsAttribute : Attribute
    {
        public FindEditableFieldsAttribute(FindFieldBy by)
        {
            By = by;
        }

        public FindFieldBy By { get; private set; }

        public FindAttribute CreateFindAttribute()
        {
            // TODO: Finish this switch.
            switch (By)
            {
                case FindFieldBy.Id:
                    return new FindByIdAttribute();
                case FindFieldBy.Label:
                    return new FindByLabelAttribute();
                default:
                    throw new InvalidOperationException("Unknown 'By' value.");
            }
        }

        public static FindAttribute CreateDefaultFindAttribute()
        {
            return new FindByLabelAttribute();
        }
    }
}
