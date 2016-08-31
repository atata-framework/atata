using System;
using System.Text;

namespace Atata
{
    public static class ILogManagerExtensions
    {
        public static void Start(this ILogManager logger, string sectionMessage)
        {
            logger.Start(new LogSection(sectionMessage));
        }

        public static void Error(this ILogManager logger, string message, string stackTrace)
        {
            StringBuilder builder = new StringBuilder(message).
                AppendLine().
                Append(stackTrace);

            logger.Error(builder.ToString(), null);
        }

        internal static void InfoWithExecutionTime(this ILogManager logger, string message, TimeSpan executionTime)
        {
            logger.Info($"{message} {Math.Floor(executionTime.TotalSeconds)}.{executionTime:fff}s");
        }

        internal static void InfoWithExecutionTimeInBrackets(this ILogManager logger, string message, TimeSpan executionTime)
        {
            logger.Info($"{message} ({Math.Floor(executionTime.TotalSeconds)}.{executionTime:fff}s)");
        }
    }
}
