#nullable enable

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
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <see cref="NLogConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder AddNLog(
        this LogConsumersBuilder builder,
        Action<LogConsumerBuilder<NLogConsumer>>? configure = null)
        =>
        builder.Add(configure);

    /// <summary>
    /// Adds the <see cref="NLogFileConsumer"/> instance that uses <c>NLog.Logger</c> class for logging into file.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <see cref="NLogFileConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder AddNLogFile(
        this LogConsumersBuilder builder,
        Action<LogConsumerBuilder<NLogFileConsumer>>? configure = null)
        =>
        builder.Add(configure);
}
