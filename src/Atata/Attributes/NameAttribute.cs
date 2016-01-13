using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Field)]
    public class NameAttribute : Attribute
    {
        public NameAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}
