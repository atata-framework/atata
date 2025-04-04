#nullable enable

using RandomizeFunc = System.Func<Atata.UIComponentMetadata, object>;

namespace Atata;

public static class ValueRandomizer
{
    private static readonly Dictionary<Type, RandomizeFunc> s_randomizers = [];

    static ValueRandomizer()
    {
        RegisterRandomizer(RandomizeString);
        RegisterRandomizer(RandomizeBool);
        RegisterNumberRandomizer<sbyte>();
        RegisterNumberRandomizer<byte>();
        RegisterNumberRandomizer<short>();
        RegisterNumberRandomizer<ushort>();
        RegisterNumberRandomizer<int>();
        RegisterNumberRandomizer<uint>();
        RegisterNumberRandomizer<long>();
        RegisterNumberRandomizer<ulong>();
        RegisterNumberRandomizer<float>();
        RegisterNumberRandomizer<double>();
        RegisterNumberRandomizer<decimal>();
    }

    public static void RegisterRandomizer<T>(Func<UIComponentMetadata, T> randomizeFunction)
    {
        randomizeFunction.CheckNotNull(nameof(randomizeFunction));

        s_randomizers[typeof(T)] = md => randomizeFunction(md)!;
    }

    private static void RegisterNumberRandomizer<T>()
        where T : struct
        =>
        s_randomizers[typeof(T)] = md => RandomizeNumber<T>(md);

    private static string RandomizeString(UIComponentMetadata metadata)
    {
        if (!TryRandomizeOneOfIncluded(metadata, out string value))
        {
            var attribute = metadata.Get<RandomizeStringSettingsAttribute>()
                ?? new RandomizeStringSettingsAttribute();

            value = Randomizer.GetString(attribute.Format, attribute.NumberOfCharacters);
        }

        return value;
    }

    private static T RandomizeNumber<T>(UIComponentMetadata metadata)
    {
        if (!TryRandomizeOneOfIncluded(metadata, out T value))
        {
            var attribute = metadata.Get<RandomizeNumberSettingsAttribute>()
                ?? new RandomizeNumberSettingsAttribute();

            decimal valueAsDecimal = Randomizer.GetDecimal(attribute.Min, attribute.Max, attribute.Precision);
            value = (T)Convert.ChangeType(valueAsDecimal, typeof(T));
        }

        return value;
    }

    private static bool RandomizeBool(UIComponentMetadata metadata) =>
        Randomizer.GetBool();

    [return: NotNull]
    private static T RandomizeNonFlagEnum<T>(Type enumType, UIComponentMetadata metadata)
    {
        var optionValues = GetEnumOptionValues<T>(enumType, metadata);
        return Randomizer.GetOneOf(optionValues);
    }

    [return: NotNull]
    private static T RandomizeFlagsEnum<T>(Type enumType, UIComponentMetadata metadata)
    {
        var optionValues = GetEnumOptionValues<T>(enumType, metadata);
        var countAttribute = metadata.Get<RandomizeCountAttribute>();

        int minCount = countAttribute?.Min ?? 1;
        int maxCount = countAttribute?.Max ?? 1;

        T[] valuesAsArray = Randomizer.GetManyOf(minCount, maxCount, optionValues);

        if (valuesAsArray.Length > 1)
        {
            Enum first = (Enum)(object)valuesAsArray[0]!;
            return (T)(object)valuesAsArray.Skip(1).Cast<Enum>().Aggregate(first, (a, b) => a.AddFlag(b));
        }
        else
        {
            return valuesAsArray[0]!;
        }
    }

    private static T[] GetEnumOptionValues<T>(Type enumType, UIComponentMetadata metadata)
    {
        T[] values = GetRandomizeIncludeValues<T>(metadata);

        if (values is [])
            values = [.. enumType.GetIndividualEnumFlags().Cast<T>()];

        var excludeAttribute = metadata.Get<RandomizeExcludeAttribute>();

        if (excludeAttribute?.Values?.Length > 0)
        {
            var valuesToExclude = excludeAttribute.Values.Cast<T>();
            values = [.. values.Except(valuesToExclude)];
        }

        return values;
    }

    private static bool TryRandomizeOneOfIncluded<T>(UIComponentMetadata metadata, out T value)
    {
        T[] includeValues = GetRandomizeIncludeValues<T>(metadata);

        if (includeValues is [])
        {
            value = default!;
            return false;
        }
        else
        {
            value = Randomizer.GetOneOf(includeValues);
            return true;
        }
    }

    private static T[] GetRandomizeIncludeValues<T>(UIComponentMetadata metadata)
    {
        var includeAttribute = metadata.Get<RandomizeIncludeAttribute>();

        return includeAttribute?.Values?.Cast<T>().ToArray() ?? [];
    }

    [return: NotNull]
    public static T GetRandom<T>(UIComponentMetadata metadata)
    {
        Type type = typeof(T);
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (s_randomizers.TryGetValue(type, out RandomizeFunc randomizeFunction))
        {
            return (T)randomizeFunction(metadata);
        }
        else if (type.IsEnum)
        {
            return type.IsDefined(typeof(FlagsAttribute), false)
                ? RandomizeFlagsEnum<T>(type, metadata)
                : RandomizeNonFlagEnum<T>(type, metadata);
        }
        else
        {
            throw new InvalidOperationException(
                $"Cannot get random value for '{type.FullName}' type. " +
                $"There is no registered randomizer for this type. " +
                $"Use {nameof(ValueRandomizer)}.{nameof(RegisterRandomizer)} method to register custom randomizer.");
        }
    }
}
