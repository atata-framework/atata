using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public class ControlFindingAttribute : Attribute
    {
        private Func<TermFindAttribute> findAttributeCreator;

        public ControlFindingAttribute(FindTermBy by)
            : this(by.ResolveFindAttributeType())
        {
        }

        public ControlFindingAttribute(Type findAttributeType)
        {
            FindAttributeType = findAttributeType.
                CheckNotNull("findAttributeType").
                Check(x => x.IsSubclassOf(typeof(TermFindAttribute)), "findAttributeType", "\"{0}\" type is not a subclass of TermFindAttribute.".FormatWith(findAttributeType.FullName));
        }

        public Type FindAttributeType { get; private set; }
        public Type ControlType { get; set; }
        public Type ParentComponentType { get; set; }

        public TermFindAttribute CreateFindAttribute()
        {
            var creator = findAttributeCreator ?? (findAttributeCreator = CreateFindAttributeCreator());
            return creator();
        }

        private Func<TermFindAttribute> CreateFindAttributeCreator()
        {
            return () => (TermFindAttribute)ActivatorEx.CreateInstance(FindAttributeType);
        }
    }
}
