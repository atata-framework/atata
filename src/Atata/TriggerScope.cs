using System;

namespace Atata
{
    /// <summary>
    /// Specifies the scope of the trigger application.
    /// </summary>
    [Obsolete("There is no more need to use this enum.")] // Obsolete since v1.8.0.
    public enum TriggerScope
    {
        /// <summary>
        /// Indicates that the trigger applies to the class/member declared.
        /// </summary>
        Self,

        /// <summary>
        /// Indicates that the trigger applies to the child control properties.
        /// </summary>
        Children
    }
}
