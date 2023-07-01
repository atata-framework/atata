namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="ILogManager"/>.
/// </summary>
public static class ILogManagerExtensions
{
    /// <summary>
    /// Executes the action within the log section with the specified section message and the <see cref="LogLevel.Info"/> level.
    /// Writes start and end log messages.
    /// Writes exception to the end log message, if it occurs.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="sectionMessage">The section message.</param>
    /// <param name="action">The action to execute.</param>
    public static void ExecuteSection(this ILogManager logger, string sectionMessage, Action action) =>
        logger.CheckNotNull(nameof(logger))
            .ExecuteSection(new LogSection(sectionMessage), action);

    /// <summary>
    /// Executes the function within the log section with the specified section message and the <see cref="LogLevel.Info"/> level.
    /// Writes start and end log messages.
    /// Writes exception to the end log message, if it occurs.
    /// Also writes result of the <paramref name="function"/> to the end log message.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="logger">The logger.</param>
    /// <param name="sectionMessage">The section message.</param>
    /// <param name="function">The function to execute.</param>
    /// <returns>The result of <paramref name="function"/>.</returns>
    public static TResult ExecuteSection<TResult>(this ILogManager logger, string sectionMessage, Func<TResult> function) =>
        logger.CheckNotNull(nameof(logger))
            .ExecuteSection(new LogSection(sectionMessage), function);

    public static void Start(this ILogManager logger, string sectionMessage) =>
        Start(logger, sectionMessage, LogLevel.Info);

    public static void Start(this ILogManager logger, string sectionMessage, LogLevel level) =>
        logger.Start(new LogSection(sectionMessage, level));

    public static void Error(this ILogManager logger, string message, string stackTrace)
    {
        StringBuilder builder = new StringBuilder(message)
            .AppendLine()
            .Append(stackTrace);

        logger.Error(builder.ToString());
    }

    internal static void InfoWithExecutionTime(this ILogManager logger, string message, TimeSpan executionTime) =>
        logger.Info($"{message} {executionTime.ToLongIntervalString()}");

    internal static void InfoWithExecutionTimeInBrackets(this ILogManager logger, string message, TimeSpan executionTime) =>
        logger.Info($"{message} ({executionTime.ToLongIntervalString()})");
}
