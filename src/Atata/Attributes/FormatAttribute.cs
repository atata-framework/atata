using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface)]
    public class FormatAttribute : Attribute
    {
        public FormatAttribute(string value = null)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}
