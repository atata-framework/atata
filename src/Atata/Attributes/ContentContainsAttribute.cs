using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContentContainsAttribute : Attribute
    {
        public ContentContainsAttribute(params string[] values)
        {
            Values = values;
        }

        public string[] Values { get; private set; }
    }
}
