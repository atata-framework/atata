using System;

namespace Atata
{
    public class FindEditableFieldsAttribute : FindControlsAttribute
    {
        public FindEditableFieldsAttribute(FindTermBy by)
            : base(typeof(EditableField<,>), by)
        {
        }

        public FindEditableFieldsAttribute(Type finderType)
            : base(typeof(EditableField<,>), finderType)
        {
        }

        public static FindAttribute CreateDefaultFindAttribute()
        {
            return new FindByLabelAttribute();
        }
    }
}
