using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public static class ILogManagerExtensions
    {
        [ThreadStatic]
        private static Stack<LogSectionInfo> sectionEndMessageStack;

        private static Stack<LogSectionInfo> SectionEndMessageStack
        {
            get { return sectionEndMessageStack ?? (sectionEndMessageStack = new Stack<LogSectionInfo>()); }
        }

        public static void StartSection(this ILogManager logger, string message, params object[] args)
        {
            string fullMessage = args != null && args.Any() ? message.FormatWith(args) : message;
            logger.Info("Starting: {0}", fullMessage);

            string endMessage = "Finished: {0}".FormatWith(fullMessage);
            SectionEndMessageStack.Push(new LogSectionInfo(endMessage));
        }

        public static void StartClickingSection(this ILogManager logger, string componentName)
        {
            logger.StartSection("Click {0}".FormatWith(componentName));
        }

        public static void StartSettingSection(this ILogManager logger, string fieldName, object value)
        {
            logger.StartSection("Set '{0}' to {1}".FormatWith(value, fieldName));
        }

        public static void StartAddingSection(this ILogManager logger, string fieldName, object value)
        {
            logger.StartSection("Add '{0}' to {1}".FormatWith(value, fieldName));
        }

        public static void StartSavingSection(this ILogManager logger, string itemKind)
        {
            logger.StartSection("Save {0}".FormatWith(itemKind));
        }

        public static void StartSavingSection(this ILogManager logger, string itemKind, string itemName)
        {
            logger.StartSection("Save '{0}' {1}".FormatWith(itemName, itemKind));
        }

        public static void StartDeletingSection(this ILogManager logger, string itemKind, string itemName)
        {
            logger.StartSection("Delete '{0}' {1}".FormatWith(itemName, itemKind));
        }

        public static void StartVerificationSection(this ILogManager logger, string message, params object[] args)
        {
            logger.StartSection("Verify " + message, args);
        }

        public static void StartVerifyingFieldSection(this ILogManager logger, string fieldName)
        {
            logger.StartSection("Verify '{0}' field".FormatWith(fieldName));
        }

        public static void StartSelectingSection(this ILogManager logger, string itemName)
        {
            logger.StartSection("Select {0}".FormatWith(itemName));
        }

        public static void EndSection(this ILogManager logger)
        {
            if (SectionEndMessageStack.Any())
            {
                LogSectionInfo sectionInfo = SectionEndMessageStack.Pop();

                TimeSpan duration = sectionInfo.GetDuration();
                logger.Info("{0} ({1}.{2:fff}s)", sectionInfo.EndMessage, Math.Floor(duration.TotalSeconds), duration);
            }
        }

        public class LogSectionInfo
        {
            public LogSectionInfo(string endMessage)
            {
                EndMessage = endMessage;
                StartedAt = DateTime.UtcNow;
            }

            public string EndMessage { get; private set; }
            public DateTime StartedAt { get; private set; }

            public TimeSpan GetDuration()
            {
                return DateTime.UtcNow - StartedAt;
            }
        }
    }
}
