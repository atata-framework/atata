using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium.Remote;

namespace Atata
{
    /// <summary>
    /// Provides a set of extension methods for <see cref="RemoteWebDriver"/>
    /// that wrap actual methods with log sections.
    /// </summary>
    public static class RemoteWebDriverLoggingExtensions
    {
        private const int ScriptMaxLength = 100;

        /// <summary>
        /// Executes the specified script within a log section.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="script">The script.</param>
        /// <param name="args">The script arguments.</param>
        /// <returns>The value returned by the script.</returns>
        public static object ExecuteScriptWithLogging(this RemoteWebDriver driver, string script, params object[] args)
        {
            driver.CheckNotNull(nameof(driver));

            ILogManager log = AtataContext.Current?.Log;

            object Execute() =>
                driver.ExecuteScript(script, args);

            if (log != null)
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
        /// <param name="driver">The driver.</param>
        /// <param name="script">The script.</param>
        /// <param name="args">The script arguments.</param>
        /// <returns>The value returned by the script.</returns>
        public static object ExecuteAsyncScriptWithLogging(this RemoteWebDriver driver, string script, params object[] args)
        {
            driver.CheckNotNull(nameof(driver));

            ILogManager log = AtataContext.Current?.Log;

            object Execute() =>
                driver.ExecuteAsyncScript(script, args);

            if (log != null)
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

        private static string BuildLogMessageForScript(string script, object[] args)
        {
            IEnumerable<string> scriptLines = script
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());

            string scriptTruncated = string.Join(" ", scriptLines)
                .Truncate(ScriptMaxLength);

            StringBuilder builder = new StringBuilder($@"""{scriptTruncated}""");

            if (args != null && args.Length > 0)
                builder.Append($" with argument{(args.Length > 1 ? "s" : null)}: {Stringifier.ToString(args)}");

            return builder.ToString();
        }
    }
}
