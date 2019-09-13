using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Atata
{
    /// <summary>
    /// Represents one or more errors that occur during an aggregate assertion.
    /// </summary>
    [Serializable]
    public class AggregateAssertionException : Exception
    {
        public AggregateAssertionException()
        {
        }

        public AggregateAssertionException(string message)
            : base(message)
        {
        }

        public AggregateAssertionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AggregateAssertionException(IEnumerable<AssertionResult> results)
            : base(ConvertToMessage(results))
        {
            Results = results.ToReadOnly();
        }

        protected AggregateAssertionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets or sets the prefix displayed at the beginning of warning result message.
        /// The default value is <c>"⚠ "</c>.
        /// </summary>
        public static string WarningResultPrefix { get; set; } = "\u26A0 ";

        /// <summary>
        /// Gets or sets the prefix displayed at the beginning of failed result message.
        /// The default value is <c>"❎ "</c>.
        /// </summary>
        public static string FailedResultPrefix { get; set; } = "\u274E ";

        /// <summary>
        /// Gets or sets the prefix displayed at the beginning of exception result message.
        /// The default value is <c>"❎ "</c>.
        /// </summary>
        public static string ExceptionResultPrefix { get; set; } = "\u274E ";

        /// <summary>
        /// Gets or sets a value indicating whether the stack trace of assertion result item should be appended to the combined message.
        /// The default value is <see langword="false"/>.
        /// </summary>
        public static bool AppendResultStackTrace { get; set; }

        /// <summary>
        /// Gets the collection of assertion results.
        /// </summary>
        public ReadOnlyCollection<AssertionResult> Results { get; } = new AssertionResult[0].ToReadOnly();

        private static string ConvertToMessage(IEnumerable<AssertionResult> results)
        {
            StringBuilder builder = new StringBuilder(BuildIntroMessage(results));

            foreach (AssertionResult result in results.Where(x => x.Status != AssertionStatus.Passed))
            {
                builder.AppendLine().AppendLine();

                string prefix = ResolveResultPrefix(result.Status);

                if (prefix != null)
                    builder.Append(prefix);

                builder.Append(result.Message);

                if (AppendResultStackTrace && !string.IsNullOrWhiteSpace(result.StackTrace))
                    builder.AppendLine().Append(result.StackTrace);
            }

            return builder.ToString();
        }

        private static string ResolveResultPrefix(AssertionStatus status)
        {
            switch (status)
            {
                case AssertionStatus.Passed:
                    return null;
                case AssertionStatus.Warning:
                    return WarningResultPrefix;
                case AssertionStatus.Failed:
                    return FailedResultPrefix;
                case AssertionStatus.Exception:
                    return ExceptionResultPrefix;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(status, nameof(status));
            }
        }

        private static string BuildIntroMessage(IEnumerable<AssertionResult> results)
        {
            int failuresCount = results.Count(x => x.Status == AssertionStatus.Failed || x.Status == AssertionStatus.Warning);
            int exceptionsCount = results.Count(x => x.Status == AssertionStatus.Exception);

            if (failuresCount == 0 && exceptionsCount == 0)
            {
                return "No failures.";
            }
            else
            {
                List<string> groupMessages = new List<string>();

                if (failuresCount > 0)
                    groupMessages.Add($"{failuresCount} assertion failure{(failuresCount > 1 ? "s" : null)}");

                if (exceptionsCount > 0)
                    groupMessages.Add($"{exceptionsCount} exception{(exceptionsCount > 1 ? "s" : null)}");

                return $"Failed with {string.Join(" and ", groupMessages)}:";
            }
        }
    }
}
