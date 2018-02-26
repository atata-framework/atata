using System;

namespace Atata
{
    /// <summary>
    /// Defines the data format to apply for the specified control.
    /// </summary>
    [Obsolete("Use [Format({FORMAT}, TargetType = typeof({CONTROL}))] instead.")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class FormatSettingsAttribute : FormatAttribute
    {
        public FormatSettingsAttribute(Type controlType, string value)
            : base(value)
        {
            ControlType = controlType;
        }

        /// <summary>
        /// Gets or sets the type of the control (e.g.: typeof(DateInput&lt;&gt;), typeof(TimeInput&lt;&gt;)).
        /// </summary>
        public Type ControlType
        {
            get { return TargetType; }
            set { TargetType = value; }
        }
    }
}
