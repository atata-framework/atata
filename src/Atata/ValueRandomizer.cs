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
            randomizeFunction.CheckNotNull("randomizeFunction");

            Randomizers[typeof(T)] = m => randomizeFunction(m);
        }

        private static void RegisterNumberRandomizer<T>()
        {
            Randomizers[typeof(T)] = m => RandomizeNumber<T>(m);
        }

        private static string RandomizeString(UIComponentMetadata metadata)
        {
            var attribute = metadata.GetFirstOrDefaultDeclaredAttribute<RandomizeStringSettingsAttribute>() ?? new RandomizeStringSettingsAttribute();

            string format = NormalizeStringFormat(attribute.Format);
            return Randomizer.GetString(format, attribute.NumberOfCharacters);
        }

        private static T RandomizeNumber<T>(UIComponentMetadata metadata)
        {
            var attribute = metadata.GetFirstOrDefaultDeclaredAttribute<RandomizeNumberSettingsAttribute>() ?? new RandomizeNumberSettingsAttribute();

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
            Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            RandomizeFunc randomizeFunction;

            if (Randomizers.TryGetValue(type, out randomizeFunction))
                return (T)randomizeFunction(metadata);
            else
                throw new InvalidOperationException("Cannot get random value for '{0}' type. There is no registered randomizer for this type.".FormatWith(typeof(T).FullName));
        }
    }
}
