using System;

namespace Atata
{
    public class FieldFindingAttribute : ControlFindingAttribute
    {
        public FieldFindingAttribute(FindTermBy by)
            : this(by.ResolveFindAttributeType())
        {
        }

        public FieldFindingAttribute(Type findAttributeType)
            : base(findAttributeType)
        {
            ControlType = typeof(Field<,>);
        }
    }
}
