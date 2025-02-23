#nullable enable

namespace Atata;

/// <summary>
/// Provides extension methods for <see cref="LogConsumerBuilder{TLogConsumer}"/>.
/// </summary>
public static class LogConsumerBuilderExtensions
{
    /// <summary>
    /// Specifies the name of the logger.
    /// </summary>
    /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The same builder instance.</returns>
    public static LogConsumerBuilder<TLogConsumer> WithLoggerName<TLogConsumer>(
        this LogConsumerBuilder<TLogConsumer> builder,
        string? loggerName)
        where TLogConsumer : INamedLogConsumer
    {
        builder.Consumer.LoggerName = loggerName;
        return builder;
    }
}
