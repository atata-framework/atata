using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public abstract class FindItemAttribute : Attribute
    {
        public abstract IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata);
    }
}
