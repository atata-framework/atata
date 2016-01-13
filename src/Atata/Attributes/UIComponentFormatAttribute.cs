using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    public class UIComponentFormatAttribute : Attribute
    {
        public UIComponentFormatAttribute(Type componentType, string value)
        {
            ComponentType = componentType;
            Value = value;
        }

        public Type ComponentType { get; private set; }
        public string Value { get; private set; }
    }
}
