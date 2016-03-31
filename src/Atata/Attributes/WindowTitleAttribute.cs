using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WindowTitleAttribute : Attribute
    {
        public WindowTitleAttribute(bool useComponentName = true)
        {
            UseComponentName = true;
        }

        public WindowTitleAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
        public bool UseComponentName { get; private set; }
    }
}
