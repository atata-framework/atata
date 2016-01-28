using System.Linq;

namespace Atata
{
    public static class StringExtensions
    {
        public static string ToUpperFirstLetter(this string value)
        {
            if (value == null)
                return null;
            else if (value.Length > 1)
                return char.ToUpper(value[0]) + value.Substring(1);
            else
                return value.ToUpper();
        }

        public static string PascalDasherize(this string underscoredWord)
        {
            string[] parts = underscoredWord.Split('-');
            return string.Join("-", parts.Select(x => x.ToUpperFirstLetter()));
        }

        public static string PascalHyphenate(this string underscoredWord)
        {
            string[] parts = underscoredWord.Split('_');
            return string.Join("‐", parts.Select(x => x.ToUpperFirstLetter()));
        }
    }
}
