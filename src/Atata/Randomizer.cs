namespace Atata;

public static class Randomizer
{
    /// <summary>
    /// The default random string length.
    /// </summary>
    public const int DefaultStringLength = 15;

    /// <summary>
    /// The default random string character set.
    /// </summary>
    public const string DefaultStringCharSet = "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Gets the random string.
    /// </summary>
    /// <param name="format">The format, that can contain <c>{0}</c> for random value insertion.</param>
    /// <param name="length">The length.</param>
    /// <returns>The random string.</returns>
    /// <exception cref="ArgumentException">
    /// The length should be positive.
    /// Or the length of string is not greater than the format length.
    /// </exception>
    public static string GetString(string format = "{0}", int length = DefaultStringLength)
    {
        if (length < 1)
            throw new ArgumentException($"The {nameof(length)} should be positive.", nameof(length));

        string normalizedFormat = NormalizeStringFormat(format);

        int randomPartLength = length - normalizedFormat.Replace("{0}", string.Empty).Length;

        if (randomPartLength <= 0)
            throw new ArgumentException($"The {nameof(length)} {length} of string is not greater than the \"{format}\" {nameof(format)} length.", nameof(length));

        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < randomPartLength; i++)
        {
            char randomChar = DefaultStringCharSet[CreateRandom().Next(0, DefaultStringCharSet.Length)];
            builder.Append(randomChar);
        }

        return string.Format(normalizedFormat, builder);
    }

    private static string NormalizeStringFormat(string format)
    {
        if (format is null or [])
            return "{0}";
        else if (!format.Contains("{0}"))
            return format + "{0}";
        else
            return format;
    }

    /// <summary>
    /// Returns a non-negative random integer that is less than the specified maximum.
    /// </summary>
    /// <param name="exclusiveMax">The exclusive upper bound of the random number to be generated. Must be greater than or equal to <c>0</c>.</param>
    /// <returns>The random <see cref="int"/> value.</returns>
    public static int GetInt(int exclusiveMax) =>
        CreateRandom().Next(exclusiveMax);

    /// <summary>
    /// Returns a random integer that is within a specified range.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number returned.</param>
    /// <param name="max">The inclusive upper bound of the random number returned. Must be greater than or equal to <paramref name="min"/>.</param>
    /// <returns>The random <see cref="int"/> value.</returns>
    public static int GetInt(int min, int max) =>
        CreateRandom().Next(min, max + 1);

    public static decimal GetDecimal(decimal min, decimal max, int precision)
    {
        var next = (decimal)CreateRandom().NextDouble();
        decimal value = min + (next * (max - min));

        return Math.Round(value, precision);
    }

    [return: NotNull]
    public static T GetEnum<T>()
    {
        var values = typeof(T).GetIndividualEnumFlags().Cast<T>();
        return GetOneOf(values);
    }

    [return: NotNull]
    public static T GetEnumExcluding<T>(params T[] valuesToExclude) =>
        GetEnumExcluding((IEnumerable<T>)valuesToExclude);

    [return: NotNull]
    public static T GetEnumExcluding<T>(IEnumerable<T> valuesToExclude)
    {
        var values = typeof(T).GetIndividualEnumFlags().Cast<T>().Except(valuesToExclude);
        return GetOneOf(values);
    }

    public static bool GetBool() =>
        CreateRandom().Next(2) == 0;

    [return: NotNull]
    public static T GetOneOf<T>(params T[] values) =>
        GetOneOf((IEnumerable<T>)values);

    [return: NotNull]
    public static T GetOneOf<T>(IEnumerable<T> values)
    {
        Guard.ThrowIfNull(values);

        IReadOnlyList<T> valueArray = values as IReadOnlyList<T> ?? [.. values];

        if (valueArray.Count == 0)
            throw new ArgumentException("Value cannot be empty.", nameof(values));

        int valueIndex = CreateRandom().Next(valueArray.Count);
        return valueArray[valueIndex]!;
    }

    public static T[] GetManyOf<T>(int count, params T[] values) =>
        GetManyOf(count, count, values);

    public static T[] GetManyOf<T>(int count, IEnumerable<T> values) =>
        GetManyOf(count, count, values);

    public static T[] GetManyOf<T>(int min, int max, params T[] values) =>
        GetManyOf(min, max, (IEnumerable<T>)values);

    public static T[] GetManyOf<T>(int min, int max, IEnumerable<T> values)
    {
        Guard.ThrowIfNullOrEmpty(values);
        Guard.ThrowIfLessThan(min, 0);
        Guard.ThrowIfLessThan(max, min);

        List<T> valuesAsList = [.. values];
        Guard.ThrowIfGreaterThan(max, valuesAsList.Count);

        int count = max == min
            ? max
            : (min + CreateRandom().Next(max + 1 - min));

        T[] randomValues = new T[count];

        if (count == 0)
            return randomValues;

        for (int i = 0; i < count; i++)
        {
            int valueIndex = CreateRandom().Next(valuesAsList.Count);
            randomValues[i] = valuesAsList[valueIndex];
            valuesAsList.Remove(randomValues[i]);
        }

        return randomValues;
    }

    private static Random CreateRandom() =>
        new(Guid.NewGuid().GetHashCode());
}
