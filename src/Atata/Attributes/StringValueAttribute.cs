using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class StringValueAttribute : Attribute
    {
        public StringValueAttribute(params string[] values)
        {
            Values = values;
        }

        public string[] Values { get; private set; }
    }
}
