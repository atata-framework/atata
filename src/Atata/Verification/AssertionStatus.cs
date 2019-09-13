namespace Atata
{
    /// <summary>
    /// Specifies the assertion status.
    /// </summary>
    public enum AssertionStatus
    {
        /// <summary>
        /// The assertion condition is passed.
        /// </summary>
        Passed,

        /// <summary>
        /// Expected assertion condition is not met but the execution was not terminated.
        /// </summary>
        Warning,

        /// <summary>
        /// The assertion condition is not met.
        /// </summary>
        Failed,

        /// <summary>
        /// The exception occured during assertion.
        /// </summary>
        Exception
    }
}
