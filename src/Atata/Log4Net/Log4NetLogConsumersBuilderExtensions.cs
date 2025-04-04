namespace Atata;

/// <summary>
/// Provides log4net extension methods for <see cref="LogConsumersBuilder"/>.
/// </summary>
public static class Log4NetLogConsumersBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="Log4NetConsumer"/> instance that uses <c>log4net.ILog</c> interface for logging.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="LogConsumerBuilder{TLogConsumer}"/> of <see cref="Log4NetConsumer"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    [Obsolete("Use AddNLog or AddNLogFile from Atata.NLog package instead.")] // Obsolete since v4.0.0.
    public static AtataContextBuilder AddLog4Net(
        this LogConsumersBuilder builder,
        Action<LogConsumerBuilder<Log4NetConsumer>>? configure = null)
        =>
        builder.Add(new Log4NetConsumer(), configure);
}
