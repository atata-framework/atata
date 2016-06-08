namespace Atata
{
    /// <summary>
    /// Specifies the scope of trigger application.
    /// </summary>
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
