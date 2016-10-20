namespace Atata
{
    /// <summary>
    /// Specifies the kind of the element selector for the waiting.
    /// </summary>
    public enum WaitBy
    {
        /// <summary>
        /// Uses the id selector kind.
        /// </summary>
        Id,

        /// <summary>
        /// Uses the name selector kind.
        /// </summary>
        Name,

        /// <summary>
        /// Uses the class selector kind.
        /// </summary>
        Class,

        /// <summary>
        /// Uses the CSS selector kind.
        /// </summary>
        Css,

        /// <summary>
        /// Uses the XPath selector kind.
        /// </summary>
        XPath
    }
}
