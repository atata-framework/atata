using System;

namespace Atata
{
    /// <summary>
    /// Defines the format to apply for the specified control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class FormatSettingsAttribute : Attribute
    {
        public FormatSettingsAttribute(Type controlType, string value)
        {
            ControlType = controlType;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the type of the control (e.g.: typeof(DateInput&lt;&gt;), typeof(TimeInput&lt;&gt;)).
        /// </summary>
        public Type ControlType { get; set; }

        /// <summary>
        /// Gets the format value.
        /// </summary>
        public string Value { get; private set; }
    }
}
