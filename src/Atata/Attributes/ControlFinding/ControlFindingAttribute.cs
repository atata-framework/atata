using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public class ControlFindingAttribute : Attribute
    {
        public ControlFindingAttribute(FindTermBy by)
            : this(by.ResolveFindAttributeType())
        {
        }

        public ControlFindingAttribute(Type findAttributeType)
        {
            FindAttributeType = findAttributeType.
                CheckNotNull(nameof(findAttributeType)).
                Check(x => x.IsSubclassOf(typeof(FindAttribute)), nameof(findAttributeType), $"{findAttributeType.FullName} type is not a subclass of {nameof(FindAttribute)}.");
        }

        public Type FindAttributeType { get; private set; }

        public Type ControlType { get; set; }

        public Type ParentComponentType { get; set; }

        public FindAttribute CreateFindAttribute()
        {
            return (FindAttribute)ActivatorEx.CreateInstance(FindAttributeType);
        }
    }
}
