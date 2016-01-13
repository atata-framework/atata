using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TitleContainsAttribute : Attribute
    {
        public TitleContainsAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}
