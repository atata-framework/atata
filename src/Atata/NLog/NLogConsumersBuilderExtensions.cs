namespace Atata;

/// <summary>
/// Provides NLog extension methods for <see cref="LogConsumersBuilder"/>.
/// </summary>
public static class NLogConsumersBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="NLogConsumer"/> instance that uses <c>NLog.Logger</c> class for logging.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public static LogConsumerBuilder<NLogConsumer> AddNLog(
        this LogConsumersBuilder builder,
        string loggerName = null)
        =>
        builder.Add(new NLogConsumer(loggerName));

    /// <summary>
    /// Adds the <see cref="NLogFileConsumer"/> instance that uses <c>NLog.Logger</c> class for logging into file.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The <see cref="LogConsumerBuilder{TLogConsumer}"/> instance.</returns>
    public static LogConsumerBuilder<NLogFileConsumer> AddNLogFile(
        this LogConsumersBuilder builder)
        =>
        builder.Add(new NLogFileConsumer());
}
