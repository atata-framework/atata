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

        internal static T CheckNotEquals<T>(this T value, string argumentName, T invalidValue, string errorMessage = null)
            where T : struct
        {
            if (Equals(value, invalidValue))
                throw new ArgumentException(ConcatMessage($"Invalid {typeof(T).FullName} value: {value}. Should not equal to: {invalidValue}.", errorMessage), argumentName);

            return value;
        }

        internal static T CheckGreaterOrEqual<T>(this T value, string argumentName, T checkValue, string errorMessage = null)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(checkValue) < 0)
                throw new ArgumentOutOfRangeException(argumentName, value, ConcatMessage($"Invalid {typeof(T).FullName} value: {value}. Should be greater or equal to: {checkValue}.", errorMessage));

            return value;
        }

        internal static int CheckIndexNonNegative(this int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("index", value, "Index was out of range. Must be non-negative.");

            return value;
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
