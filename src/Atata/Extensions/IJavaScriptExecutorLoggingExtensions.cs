#nullable enable

namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="IJavaScriptExecutor"/>
/// that wrap actual methods with log sections.
/// </summary>
public static class IJavaScriptExecutorLoggingExtensions
{
    private const int ScriptMaxLengthForLog = 100;

    /// <summary>
    /// Executes the specified script within a log section.
    /// </summary>
    /// <param name="scriptExecutor">The script executor.</param>
    /// <param name="script">The script.</param>
    /// <param name="args">The script arguments.</param>
    /// <returns>The value returned by the script.</returns>
    public static object? ExecuteScriptWithLogging(this IJavaScriptExecutor scriptExecutor, string script, params object?[] args) =>
        scriptExecutor.ExecuteScriptWithLogging(
            AtataContext.Current?.Sessions.GetOrNull<WebDriverSession>()?.Log,
            script,
            args);

    internal static object? ExecuteScriptWithLogging(this IJavaScriptExecutor scriptExecutor, ILogManager? log, string script, params object?[] args)
    {
        scriptExecutor.CheckNotNull(nameof(scriptExecutor));

        object? Execute() =>
            scriptExecutor.ExecuteScript(script, args);

        if (log is not null)
        {
            string logMessage = $"Execute script {BuildLogMessageForScript(script, args)}";

            return log.ExecuteSection(
                new LogSection(logMessage, LogLevel.Trace),
                Execute);
        }
        else
        {
            return Execute();
        }
    }

    /// <summary>
    /// Executes the specified async script within a log section.
    /// </summary>
    /// <param name="scriptExecutor">The script executor.</param>
    /// <param name="script">The script.</param>
    /// <param name="args">The script arguments.</param>
    /// <returns>The value returned by the script.</returns>
    public static object? ExecuteAsyncScriptWithLogging(this IJavaScriptExecutor scriptExecutor, string script, params object?[] args) =>
        scriptExecutor.ExecuteAsyncScriptWithLogging(
            AtataContext.Current?.Sessions.GetOrNull<WebDriverSession>()?.Log,
            script,
            args);

    internal static object? ExecuteAsyncScriptWithLogging(this IJavaScriptExecutor scriptExecutor, ILogManager? log, string script, params object?[] args)
    {
        scriptExecutor.CheckNotNull(nameof(scriptExecutor));

        object? Execute() =>
            scriptExecutor.ExecuteAsyncScript(script, args);

        if (log is not null)
        {
            string logMessage = $"Execute async script {BuildLogMessageForScript(script, args)}";

            return log.ExecuteSection(
                new LogSection(logMessage, LogLevel.Trace),
                Execute);
        }
        else
        {
            return Execute();
        }
    }

    private static string BuildLogMessageForScript(string script, object?[] args)
    {
        IEnumerable<string> scriptLines = script
            .Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim());

        string scriptTruncated = string.Join(" ", scriptLines)
            .Truncate(ScriptMaxLengthForLog);

        var builder = new StringBuilder()
            .Append('"')
            .Append(scriptTruncated)
            .Append('"');

        if (args?.Length > 0)
            builder.Append($" with argument{(args.Length > 1 ? "s" : null)}: {Stringifier.ToStringInFormOfOneOrMany(args)}");

        return builder.ToString();
    }
}
