using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public static class ILogManagerExtensions
    {
        [ThreadStatic]
        private static readonly Stack<string> SectionEndMessageStack = new Stack<string>();

        public static void StartSection(this ILogManager logger, string message, params object[] args)
        {
            string fullMessage = args != null && args.Any() ? message.FormatWith(args) : message;
            logger.Info("Starting: {0}", fullMessage);
            SectionEndMessageStack.Push("Finished: {0}".FormatWith(fullMessage));
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
                string message = SectionEndMessageStack.Pop();
                logger.Info(message);
            }
        }
    }
}
