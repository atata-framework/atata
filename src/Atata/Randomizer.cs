using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public static int GetInt(int min, int max)
        {
            return new Random().Next(min, max);
        }

        public static decimal GetDecimal(decimal min, decimal max, int precision)
        {
            var next = (decimal)new Random().NextDouble();
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
            return new Random().Next(2) == 0;
        }

        public static T GetOneOf<T>(params T[] values)
        {
            return GetOneOf((IEnumerable<T>)values);
        }

        public static T GetOneOf<T>(IEnumerable<T> values)
        {
            values.CheckNotNullOrEmpty(nameof(values));

            int valueIndex = new Random().Next(values.Count());
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

            int count = max == min ? max : (min + new Random().Next(max + 1 - min));

            T[] randomValues = new T[count];

            if (count == 0)
                return randomValues;

            for (int i = 0; i < count; i++)
            {
                int valueIndex = new Random().Next(valuesAsList.Count);
                randomValues[i] = valuesAsList[valueIndex];
                valuesAsList.Remove(randomValues[i]);
            }

            return randomValues;
        }
    }
}
