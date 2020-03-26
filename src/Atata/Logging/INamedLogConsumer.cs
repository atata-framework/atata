namespace Atata
{
    /// <summary>
    /// Represents the log consumer that provides the logger name.
    /// </summary>
    public interface INamedLogConsumer : ILogConsumer
    {
        /// <summary>
        /// Gets or sets the name of the logger.
        /// </summary>
        string LoggerName { get; set; }
    }
}
