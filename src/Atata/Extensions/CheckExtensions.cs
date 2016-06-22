using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atata
{
    internal static class CheckExtensions
    {
        internal static T Check<T>(this T value, Predicate<T> checkPredicate, string argumentName, string errorMessage = null)
        {
            if (checkPredicate != null && !checkPredicate(value))
                throw new ArgumentException(errorMessage, argumentName);

            return value;
        }

        internal static T CheckNotNull<T>(this T value, string argumentName, string errorMessage = null)
        {
            if (value == null)
                throw new ArgumentNullException(argumentName, errorMessage);

            return value;
        }

        internal static string CheckNotNullOrEmpty(this string value, string argumentName, string errorMessage = null)
        {
            if (value == null)
                throw new ArgumentNullException(argumentName, errorMessage);
            if (value == string.Empty)
                throw new ArgumentException(ConcatMessage("Should not be empty string.", errorMessage), argumentName);

            return value;
        }

        internal static string CheckNotNullOrWhitespace(this string value, string argumentName, string errorMessage = null)
        {
            if (value == null)
                throw new ArgumentNullException(argumentName, errorMessage);
            if (value == string.Empty)
                throw new ArgumentException(ConcatMessage("Should not be empty string or whitespace.", errorMessage), argumentName);

            return value;
        }

        internal static IEnumerable<T> CheckNotNullOrEmpty<T>(this IEnumerable<T> collection, string argumentName, string errorMessage = null)
        {
            if (collection == null)
                throw new ArgumentNullException(argumentName, errorMessage);
            if (!collection.Any())
                throw new ArgumentException(ConcatMessage("Collection should contain at least one element.", errorMessage), argumentName);

            return collection;
        }

        private static string ConcatMessage(string primaryMessage, string secondaryMessage)
        {
            StringBuilder builder = new StringBuilder(primaryMessage);

            if (!string.IsNullOrEmpty(secondaryMessage))
                builder.AppendFormat(" {0}", secondaryMessage);

            return builder.ToString();
        }
    }
}
