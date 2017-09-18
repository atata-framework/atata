namespace Atata
{
    /// <summary>
    /// Specifies the mode of the <see cref="AtataContext"/>.
    /// </summary>
    public enum AtataContextMode
    {
        /// <summary>
        /// <see cref="AtataContext.Current"/> uses thread-static (unique for each thread) value. Is good for environment with parallel test execution.
        /// </summary>
        ThreadStatic,

        /// <summary>
        /// <see cref="AtataContext.Current"/> uses static (common for all threads) value. Is good for environment with asynchronous actions, but without parallel test execution.
        /// </summary>
        Static
    }
}
