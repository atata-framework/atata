using System;

namespace Atata
{
    /// <summary>
    /// Defines the data format to apply for the specified control.
    /// </summary>
    [Obsolete("Use [Format({FORMAT}, TargetType = typeof({CONTROL}))] instead.")] // Obsolete since v0.17.0.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class FormatSettingsAttribute : FormatAttribute
    {
        public FormatSettingsAttribute(Type controlType, string value)
            : base(value)
        {
            ControlType = controlType;
        }

        /// <summary>
        /// Gets or sets the type of the control (e.g.: <c>typeof(DateInput&lt;&gt;)</c>, <c>typeof(TimeInput&lt;&gt;)</c>).
        /// </summary>
        public Type ControlType
        {
            get { return TargetType; }
            set { TargetType = value; }
        }
    }
}
