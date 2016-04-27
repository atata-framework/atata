using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class FindControlsAttribute : Attribute
    {
        private Func<TermFindAttribute> findAttributeCreator;

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
            var creator = GetFindAttributeCreator();
            return creator();
        }

        private Func<TermFindAttribute> GetFindAttributeCreator()
        {
            return findAttributeCreator != null ? findAttributeCreator : findAttributeCreator = CreateFindAttributeCreator();
        }

        private Func<TermFindAttribute> CreateFindAttributeCreator()
        {
            if (FinderType == null)
                throw new InvalidOperationException("FinderType is not set.");

            if (!FinderType.IsSubclassOf(typeof(TermFindAttribute)))
                throw new InvalidOperationException("'{0}' FinderType is not subclass of TermFindAttribute.".FormatWith(FinderType.FullName));

            var constructor = FinderType.GetConstructor(new[] { typeof(string[]) });
            if (constructor != null)
                return () => (TermFindAttribute)constructor.Invoke(new object[] { null });

            constructor = FinderType.GetConstructor(Type.EmptyTypes);
            if (constructor != null)
                return () => (TermFindAttribute)constructor.Invoke(null);

            throw new InvalidOperationException("'{0}' FinderType doesn't have a parameterless constructor.".FormatWith(FinderType.FullName));
        }
    }
}
