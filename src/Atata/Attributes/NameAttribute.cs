using System;

namespace Atata
{
    /// <summary>
    /// Specifies the name of the component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Field)]
    public class NameAttribute : Attribute
    {
        public NameAttribute(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the name value.
        /// </summary>
        public string Value { get; }
    }
}
