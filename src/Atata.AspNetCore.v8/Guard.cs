namespace Atata;

internal static class Guard
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    internal static T ReturnOrThrowIfNull<T>([NotNull] T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ThrowIfNull(argument, paramName);
        return argument;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    internal static string ReturnOrThrowIfNullOrWhitespace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ThrowIfNullOrWhitespace(argument, paramName);
        return argument;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    internal static string ReturnOrThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ThrowIfNullOrEmpty(argument, paramName);
        return argument;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfNull<T>([NotNull] T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfNullOrWhitespace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);

        if (string.IsNullOrWhiteSpace(argument))
            throw new ArgumentException("Value cannot be an empty string or composed entirely of whitespace.", paramName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);

        if (argument.Length == 0)
            throw new ArgumentException("Value cannot be an empty string.", paramName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfNullOrEmpty<T>([NotNull] IEnumerable<T> argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);

        bool isEmpty = argument is IReadOnlyCollection<T> collectionCasted
            ? collectionCasted.Count == 0
            : !argument.Any();

        if (isEmpty)
            throw new ArgumentException("Value cannot be empty.", paramName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfGreaterThan<T>(T argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        if (argument?.CompareTo(other) > 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be greater than {other}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfGreaterThan<T>(T? argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        if (argument?.CompareTo(other) > 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be greater than {other}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfGreaterThanOrEqualTo<T>(T argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        if (argument?.CompareTo(other) >= 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be greater than or equal to {other}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfGreaterThanOrEqualTo<T>(T? argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        if (argument?.CompareTo(other) >= 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be greater than or equal to {other}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfLessThan<T>(T argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        if (argument?.CompareTo(other) < 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be less than {other}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfLessThan<T>(T? argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        if (argument?.CompareTo(other) < 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be less than {other}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfLessThanOrEqualTo<T>(T argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        if (argument?.CompareTo(other) <= 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be less than or equal to {other}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfLessThanOrEqualTo<T>(T? argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        if (argument?.CompareTo(other) <= 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be less than or equal to {other}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfIndexIsNegative(int index)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), index, "Index was out of range. Must be non-negative.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfIndexIsGreaterOrEqualTo(int index, int size)
    {
        if (index >= size)
            throw new ArgumentOutOfRangeException(nameof(index), index, $"Index was out of range. Must be less than the size of the collection, which is {size}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfIndexIsNegativeOrGreaterOrEqualTo(int index, int size)
    {
        ThrowIfIndexIsNegative(index);
        ThrowIfIndexIsGreaterOrEqualTo(index, size);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfNot<T>([NotNull] Type argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ThrowIfNull(argument, paramName);

        Type expectedType = typeof(T);

        if (!expectedType.IsAssignableFrom(argument))
            throw new ArgumentException(
                $"Type {argument.FullName} is not {(argument.IsClass && expectedType.IsClass ? "inherited from" : "assignable to")} {expectedType.FullName}.", paramName);
    }

    internal static ArgumentException CreateArgumentExceptionForUnsupportedValue<T>(
        T argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        Type originalType = typeof(T);
        Type underlyingType = argument is not null
            ? Nullable.GetUnderlyingType(originalType) ?? originalType
            : originalType;

        bool isEnum = underlyingType.IsEnum;

        StringBuilder builder = new("Unsupported ");

        builder.Append(underlyingType.FullName);
        builder.Append(' ');

        if (isEnum)
        {
            builder.Append("enum ");

            if (argument is not null && !underlyingType.IsEnumDefined(argument))
                builder.Append("undefined ");
        }

        builder.Append("value: ");
        builder.Append(argument is null ? "null" : argument.ToString());
        builder.Append('.');

        return new(builder.ToString(), paramName);
    }
}
