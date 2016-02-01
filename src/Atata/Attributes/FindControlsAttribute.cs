using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class FindControlsAttribute : Attribute
    {
        public FindControlsAttribute(Type controlType, FindTermBy by)
            : this(controlType, by.ResolveFindAttributeType())
        {
        }

        public FindControlsAttribute(Type controlType, Type finderType)
        {
            ControlType = controlType;
            FinderType = finderType;
        }

        public Type ControlType { get; private set; }
        public Type FinderType { get; private set; }
        public Type ParentControlType { get; set; }

        public TermFindAttribute CreateFindAttribute()
        {
            return (TermFindAttribute)Activator.CreateInstance(FinderType);
        }
    }
}
