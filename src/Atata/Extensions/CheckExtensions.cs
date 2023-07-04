namespace Atata;

internal static class CheckExtensions
{
    internal static T Check<T>(this T value, Predicate<T> checkPredicate, string argumentName, string errorMessage)
    {
        if (checkPredicate is not null && !checkPredicate(value))
            throw new ArgumentException(errorMessage, argumentName);

        return value;
    }

    internal static T CheckNotNull<T>(this T value, string argumentName, string errorMessage = null)
    {
        if (value is null)
            throw CreateArgumentNullException(argumentName, errorMessage);

        return value;
    }

    internal static string CheckNotNullOrWhitespace(this string value, string argumentName, string errorMessage = null)
    {
        if (value is null)
            throw CreateArgumentNullException(argumentName, errorMessage);
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(ConcatMessage("Should not be empty string or whitespace.", errorMessage), argumentName);

        return value;
    }

    internal static string CheckNotNullOrEmpty(this string value, string argumentName, string errorMessage = null)
    {
        if (value is null)
            throw CreateArgumentNullException(argumentName, errorMessage);
        if (value.Length == 0)
            throw new ArgumentException(ConcatMessage("Should not be empty string.", errorMessage), argumentName);

        return value;
    }

    internal static IEnumerable<T> CheckNotNullOrEmpty<T>(this IEnumerable<T> collection, string argumentName, string errorMessage = null)
    {
        if (collection is null)
            throw CreateArgumentNullException(argumentName, errorMessage);

        bool isEmpty = collection is IReadOnlyCollection<T> collectionCasted
            ? collectionCasted.Count == 0
            : !collection.Any();

        if (isEmpty)
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

    internal static T CheckLessOrEqual<T>(this T value, string argumentName, T checkValue, string errorMessage = null)
        where T : struct, IComparable<T>
    {
        if (value.CompareTo(checkValue) > 0)
            throw new ArgumentOutOfRangeException(argumentName, value, ConcatMessage($"Invalid {typeof(T).FullName} value: {value}. Should be less or equal to: {checkValue}.", errorMessage));

        return value;
    }

    internal static int CheckIndexNonNegative(this int index)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), index, "Index was out of range. Must be non-negative.");

        return index;
    }

    internal static Type CheckIs<T>(this Type value, string argumentName, string errorMessage = null)
    {
        value.CheckNotNull(argumentName);

        Type expectedType = typeof(T);

        if (!expectedType.IsAssignableFrom(value))
            throw new ArgumentException(ConcatMessage($"{value.FullName} type should be assignable to {expectedType.FullName}.", errorMessage), argumentName);

        return value;
    }

    private static ArgumentNullException CreateArgumentNullException(string argumentName, string message) =>
        message is null
        ? new ArgumentNullException(argumentName)
        : new ArgumentNullException(argumentName, message);

    private static string ConcatMessage(string primaryMessage, string secondaryMessage) =>
        string.IsNullOrEmpty(secondaryMessage)
            ? primaryMessage
            : $"{primaryMessage} {secondaryMessage}";
}
