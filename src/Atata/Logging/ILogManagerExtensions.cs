using System;
using System.Text;

namespace Atata
{
    public static class ILogManagerExtensions
    {
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
}
