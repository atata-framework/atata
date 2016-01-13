using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public abstract class FindAttribute : Attribute
    {
        protected FindAttribute()
        {
        }

        public int Index { get; set; }

        public abstract IElementFindStrategy CreateStrategy(UIPropertyMetadata metadata);
    }
}
