using System;

namespace Atata
{
    /// <summary>
    /// Specifies the format of a control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface)]
    public class FormatAttribute : Attribute
    {
        public FormatAttribute(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the format value.
        /// </summary>
        public string Value { get; private set; }
    }
}
