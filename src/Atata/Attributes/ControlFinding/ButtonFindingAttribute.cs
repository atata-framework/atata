using System;

namespace Atata
{
    public class ButtonFindingAttribute : ControlFindingAttribute
    {
        public ButtonFindingAttribute(FindTermBy by)
            : this(by.ResolveFindAttributeType())
        {
        }

        public ButtonFindingAttribute(Type findAttributeType)
            : base(findAttributeType)
        {
            ControlType = typeof(ButtonControl<,>);
        }
    }
}
