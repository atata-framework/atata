using System;

namespace Atata
{
    public static class ValueGenerator
    {
        public static string GenerateString(string prefix = null, int numberOfCharacters = 15, string separator = " ")
        {
            string id = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, numberOfCharacters);
            return string.Format("{0}{1}{2}", prefix, separator, id);
        }

        public static int GenerateInt(int min, int max)
        {
            return new Random().Next(min, max);
        }

        public static decimal GenerateDecimal(decimal min, decimal max, int precision)
        {
            var next = (decimal)new Random().NextDouble();
            decimal value = min + (next * (max - min));

            return Math.Round(value, precision);
        }
    }
}
