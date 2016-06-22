using System;

namespace Atata
{
    public class EditableFieldFindingAttribute : ControlFindingAttribute
    {
        public EditableFieldFindingAttribute(FindTermBy by)
            : this(by.ResolveFindAttributeType())
        {
        }

        public EditableFieldFindingAttribute(Type findAttributeType)
            : base(findAttributeType)
        {
            ControlType = typeof(EditableField<,>);
        }
    }
}
