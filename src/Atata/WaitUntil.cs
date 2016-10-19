namespace Atata
{
    /// <summary>
    /// Specifies the waiting approach.
    /// </summary>
    public enum WaitUntil
    {
        /// <summary>
        /// Waits until the element will be missing.
        /// </summary>
        Missing,

        /// <summary>
        /// Waits until the element will be hidden.
        /// </summary>
        Hidden,

        /// <summary>
        /// Waits until the element will be missing or hidden.
        /// </summary>
        MissingOrHidden,

        /// <summary>
        /// Waits until the element will be visible.
        /// </summary>
        Visible,

        /// <summary>
        /// Waits until the element will exist.
        /// </summary>
        Exists,

        /// <summary>
        /// Waits until the element will be visible and than until it will be hidden.
        /// </summary>
        VisibleAndHidden,

        /// <summary>
        /// Waits until the element will be visible and than until it will be missing.
        /// </summary>
        VisibleAndMissing,

        /// <summary>
        /// Waits until the element will be missing and than until it will be visible.
        /// </summary>
        MissingAndVisible,

        /// <summary>
        /// Waits until the element will be hidden and than until it will be visible.
        /// </summary>
        HiddenAndVisible
    }
}
