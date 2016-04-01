using System;
using System.Globalization;
using System.Text;

namespace Atata
{
    public static class Randomizer
    {
        public static string GetString(string format = "{0}", int numberOfCharacters = 15)
        {
            string uniqueValue = Guid.NewGuid().ToString("N").Substring(0, numberOfCharacters);

            StringBuilder stringBuilder = new StringBuilder();
            int baseCharacter = (int)'a';
            for (int i = 0; i < uniqueValue.Length; i++)
            {
                string characterAsString = uniqueValue.Substring(i, 1);
                int characterIntValue = int.Parse(characterAsString.ToString(), NumberStyles.HexNumber);
                char alphaCharacter = (char)(baseCharacter + characterIntValue);
                stringBuilder.Append(alphaCharacter);
            }
            return string.Format(format, stringBuilder.ToString());
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
    }
}
