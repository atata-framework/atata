using System;

namespace Atata
{
    public class LinkFindingAttribute : ControlFindingAttribute
    {
        public LinkFindingAttribute(FindTermBy by)
            : this(by.ResolveFindAttributeType())
        {
        }

        public LinkFindingAttribute(Type findAttributeType)
            : base(findAttributeType)
        {
            ControlType = typeof(LinkControl<,>);
        }
    }
}
