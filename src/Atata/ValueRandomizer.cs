using System;
using System.Collections.Generic;
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

        public static T GetRandom<T>(UIComponentMetadata metadata)
        {
            Type type = typeof(T);
            type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            RandomizeFunc randomizeFunction;

            if (Randomizers.TryGetValue(type, out randomizeFunction))
                return (T)randomizeFunction(metadata);
            else
                throw new InvalidOperationException($"Cannot get random value for '{type.FullName}' type. There is no registered randomizer for this type. Use {nameof(ValueRandomizer)}.{nameof(RegisterRandomizer)} method to register custom randomizer.");
        }
    }
}
