using System;
using System.Collections.Generic;
using System.Linq;
using RandomizeFunc = System.Func<Atata.UIComponentMetadata, object>;

namespace Atata
{
    public static class ValueRandomizer
    {
        private static readonly Dictionary<Type, RandomizeFunc> Randomizers;

        static ValueRandomizer()
        {
            Randomizers = new Dictionary<Type, RandomizeFunc>();

            RegisterRandomizer(RandomizeString);
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

            Randomizers[typeof(T)] = md => randomizeFunction(md);
        }

        private static void RegisterNumberRandomizer<T>()
        {
            Randomizers[typeof(T)] = md => RandomizeNumber<T>(md);
        }

        private static string RandomizeString(UIComponentMetadata metadata)
        {
            var attribute = metadata.Get<RandomizeStringSettingsAttribute>(AttributeLevels.Declared) ?? new RandomizeStringSettingsAttribute();

            string format = NormalizeStringFormat(attribute.Format);
            return Randomizer.GetString(format, attribute.NumberOfCharacters);
        }

        private static T RandomizeNumber<T>(UIComponentMetadata metadata)
        {
            var attribute = metadata.Get<RandomizeNumberSettingsAttribute>(AttributeLevels.Declared) ?? new RandomizeNumberSettingsAttribute();

            decimal value = Randomizer.GetDecimal(attribute.Min, attribute.Max, attribute.Precision);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        private static string NormalizeStringFormat(string format)
        {
            if (string.IsNullOrEmpty(format))
                return "{0}";
            else if (!format.Contains("{0}"))
                return format + "{0}";
            else
                return format;
        }

        private static T RandomizeNonFlagEnum<T>(Type enumType, UIComponentMetadata metadata)
        {
            var optionValues = GetEnumOptionValues<T>(enumType, metadata);
            return Randomizer.GetOneOf(optionValues);
        }

        private static T RandomizeFlagsEnum<T>(Type enumType, UIComponentMetadata metadata)
        {
            var optionValues = GetEnumOptionValues<T>(enumType, metadata);
            var countAttribute = metadata.Get<RandomizeCountAttribute>(AttributeLevels.Declared);

            int minCount = countAttribute?.Min ?? 1;
            int maxCount = countAttribute?.Max ?? 1;

            T[] valuesAsArray = Randomizer.GetManyOf(minCount, maxCount, optionValues);

            if (valuesAsArray.Length > 1)
            {
                Enum first = (Enum)(object)valuesAsArray[0];
                return (T)(object)valuesAsArray.Skip(1).Cast<Enum>().Aggregate(first, (a, b) => a.AddFlag(b));
            }
            else
            {
                return valuesAsArray[0];
            }
        }

        private static T[] GetEnumOptionValues<T>(Type enumType, UIComponentMetadata metadata)
        {
            var includeAttribute = metadata.Get<RandomizeIncludeAttribute>(AttributeLevels.Declared);
            var excludeAttribute = metadata.Get<RandomizeExcludeAttribute>(AttributeLevels.Declared);

            T[] values = includeAttribute?.Values?.Cast<T>()?.ToArray();
            if (values == null || values.Length == 0)
                values = enumType.GetIndividualEnumFlags().Cast<T>().ToArray();

            if (excludeAttribute?.Values?.Any() ?? false)
            {
                var valuesToExclude = excludeAttribute.Values.Cast<T>();
                values = values.Except(valuesToExclude).ToArray();
            }

            return values;
        }

        public static T GetRandom<T>(UIComponentMetadata metadata)
        {
            Type type = typeof(T);
            type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            RandomizeFunc randomizeFunction;

            if (Randomizers.TryGetValue(type, out randomizeFunction))
            {
                return (T)randomizeFunction(metadata);
            }
            else if (type.IsEnum)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false))
                    return RandomizeFlagsEnum<T>(type, metadata);
                else
                    return RandomizeNonFlagEnum<T>(type, metadata);
            }
            else
            {
                throw new InvalidOperationException($"Cannot get random value for '{type.FullName}' type. There is no registered randomizer for this type. Use {nameof(ValueRandomizer)}.{nameof(RegisterRandomizer)} method to register custom randomizer.");
            }
        }
    }
}
