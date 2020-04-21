using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Atata
{
    public static class Randomizer
    {
        public static string GetString(string format = "{0}", int numberOfCharacters = 15)
        {
            string uniqueValue = Guid.NewGuid().ToString("N").Substring(0, numberOfCharacters);

            StringBuilder stringBuilder = new StringBuilder();
            int baseCharacter = 'a';

            for (int i = 0; i < uniqueValue.Length; i++)
            {
                string characterAsString = uniqueValue.Substring(i, 1);
                int characterIntValue = int.Parse(characterAsString, NumberStyles.HexNumber);
                char alphaCharacter = (char)(baseCharacter + characterIntValue);
                stringBuilder.Append(alphaCharacter);
            }

            return string.Format(format, stringBuilder);
        }

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="exclusiveMax">The exclusive upper bound of the random number to be generated. Must be greater than or equal to <c>0</c>.</param>
        /// <returns>The random <see cref="int"/> value.</returns>
        public static int GetInt(int exclusiveMax)
        {
            return CreateRandom().Next(exclusiveMax);
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The inclusive upper bound of the random number returned. Must be greater than or equal to <paramref name="min"/>.</param>
        /// <returns>The random <see cref="int"/> value.</returns>
        public static int GetInt(int min, int max)
        {
            return CreateRandom().Next(min, max + 1);
        }

        public static decimal GetDecimal(decimal min, decimal max, int precision)
        {
            var next = (decimal)CreateRandom().NextDouble();
            decimal value = min + (next * (max - min));

            return Math.Round(value, precision);
        }

        public static T GetEnum<T>()
        {
            var values = typeof(T).GetIndividualEnumFlags().Cast<T>();
            return GetOneOf(values);
        }

        public static T GetEnumExcluding<T>(params T[] valuesToExclude)
        {
            return GetEnumExcluding((IEnumerable<T>)valuesToExclude);
        }

        public static T GetEnumExcluding<T>(IEnumerable<T> valuesToExclude)
        {
            var values = typeof(T).GetIndividualEnumFlags().Cast<T>().Except(valuesToExclude);
            return GetOneOf(values);
        }

        public static bool GetBool()
        {
            return CreateRandom().Next(2) == 0;
        }

        public static T GetOneOf<T>(params T[] values)
        {
            return GetOneOf((IEnumerable<T>)values);
        }

        public static T GetOneOf<T>(IEnumerable<T> values)
        {
            values.CheckNotNullOrEmpty(nameof(values));

            int valueIndex = CreateRandom().Next(values.Count());
            return values.ElementAt(valueIndex);
        }

        public static T[] GetManyOf<T>(int count, params T[] values)
        {
            return GetManyOf(count, count, values);
        }

        public static T[] GetManyOf<T>(int count, IEnumerable<T> values)
        {
            return GetManyOf(count, count, values);
        }

        public static T[] GetManyOf<T>(int min, int max, params T[] values)
        {
            return GetManyOf(min, max, (IEnumerable<T>)values);
        }

        public static T[] GetManyOf<T>(int min, int max, IEnumerable<T> values)
        {
            values.CheckNotNullOrEmpty(nameof(values));
            min.CheckGreaterOrEqual(nameof(min), 0);
            max.CheckGreaterOrEqual(nameof(min), min);

            List<T> valuesAsList = values.ToList();
            max.CheckLessOrEqual(nameof(max), valuesAsList.Count, $"Count of {nameof(values)} is {valuesAsList.Count}");

            int count = max == min ? max : (min + CreateRandom().Next(max + 1 - min));

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Random CreateRandom()
        {
            return new Random(Guid.NewGuid().GetHashCode());
        }
    }
}
