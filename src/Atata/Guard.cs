namespace Atata;

internal static class Guard
{
#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [return: NotNull]
    internal static T ReturnOrThrowIfNull<T>([NotNull] T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ThrowIfNull(argument, paramName);
        return argument;
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [return: NotNull]
    internal static string ReturnOrThrowIfNullOrWhitespace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ThrowIfNullOrWhitespace(argument, paramName);
        return argument;
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [return: NotNull]
    internal static string ReturnOrThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ThrowIfNullOrEmpty(argument, paramName);
        return argument;
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfNull<T>([NotNull] T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfBothNull<T1, T2>(
        T1 argument1,
        T2 argument2,
        [CallerArgumentExpression(nameof(argument1))] string? param1Name = null,
        [CallerArgumentExpression(nameof(argument2))] string? param2Name = null)
    {
        if (argument1 is null && argument2 is null)
            throw new ArgumentNullException($"Both '{param1Name}' and '{param2Name}' values cannot be null.", null as Exception);
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfNullOrWhitespace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);

        if (string.IsNullOrWhiteSpace(argument))
            throw new ArgumentException("Value cannot be an empty string or composed entirely of whitespace.", paramName);
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);

        if (argument.Length == 0)
            throw new ArgumentException("Value cannot be an empty string.", paramName);
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfGreaterThan<T>(T argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        if (argument?.CompareTo(other) > 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be greater than {other}.");
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfGreaterThan<T>(T? argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        if (argument?.CompareTo(other) > 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be greater than {other}.");
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfGreaterThanOrEqualTo<T>(T argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        if (argument?.CompareTo(other) >= 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be greater than or equal to {other}.");
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfGreaterThanOrEqualTo<T>(T? argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        if (argument?.CompareTo(other) >= 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be greater than or equal to {other}.");
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfLessThan<T>(T argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        if (argument?.CompareTo(other) < 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be less than {other}.");
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfLessThan<T>(T? argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        if (argument?.CompareTo(other) < 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be less than {other}.");
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfLessThanOrEqualTo<T>(T argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        if (argument?.CompareTo(other) <= 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be less than or equal to {other}.");
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfLessThanOrEqualTo<T>(T? argument, T other, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        if (argument?.CompareTo(other) <= 0)
            throw new ArgumentOutOfRangeException(paramName, argument, $"Value cannot be less than or equal to {other}.");
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfIndexIsNegative(int index)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), index, "Index was out of range. Must be non-negative.");
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfIndexIsGreaterOrEqualTo(int index, int size)
    {
        if (index >= size)
            throw new ArgumentOutOfRangeException(nameof(index), index, $"Index was out of range. Must be less than the size of the collection, which is {size}.");
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfIndexIsNegativeOrGreaterOrEqualTo(int index, int size)
    {
        ThrowIfIndexIsNegative(index);
        ThrowIfIndexIsGreaterOrEqualTo(index, size);
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static void ThrowIfNot<T>([NotNull] Type argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ThrowIfNull(argument, paramName);

        Type expectedType = typeof(T);

        if (!expectedType.IsAssignableFrom(argument))
            throw new ArgumentException(
                $"Type {argument.FullName} is not {(argument.IsClass && expectedType.IsClass ? "inherited from" : "assignable to")} {expectedType.FullName}.", paramName);
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [SuppressMessage("Maintainability", "CA1513:Use ObjectDisposedException throw helper")]
    internal static void ThrowIfDisposed([DoesNotReturnIf(true)] bool condition, object instance)
    {
        if (condition)
            throw new ObjectDisposedException(instance.GetType().FullName);
    }

#if NET8_0_OR_GREATER
    [StackTraceHidden]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [SuppressMessage("Maintainability", "CA1513:Use ObjectDisposedException throw helper")]
    internal static void ThrowIfDisposed([DoesNotReturnIf(true)] bool condition, Type type)
    {
        if (condition)
            throw new ObjectDisposedException(type.FullName);
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
