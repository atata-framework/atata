using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class TermAttribute : Attribute
    {
        public TermAttribute(params string[] values)
        {
            Values = values;
        }

        public string[] Values { get; private set; }
    }
}
