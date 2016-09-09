using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class FormatSettingsAttribute : Attribute
    {
        public FormatSettingsAttribute(Type componentType, string value)
        {
            ComponentType = componentType;
            Value = value;
        }

        public Type ComponentType { get; private set; }

        public string Value { get; private set; }
    }
}
