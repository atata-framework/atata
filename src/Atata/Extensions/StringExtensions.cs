using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public static class StringExtensions
    {
        internal static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static bool IsUpper(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return value.ToCharArray().All(x => char.IsUpper(x));
        }

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

        public static string[] SplitIntoWords(this string value)
        {
            char[] chars = value.ToCharArray();

            List<char> wordChars = new List<char>();
            List<string> words = new List<string>();

            if (char.IsLetterOrDigit(chars[0]))
                wordChars.Add(chars[0]);

            Action endWord = () =>
            {
                if (wordChars.Any())
                {
                    words.Add(new string(wordChars.ToArray()));
                    wordChars.Clear();
                }
            };

            for (int i = 1; i < chars.Length; i++)
            {
                char current = chars[i];
                char prev = chars[i - 1];
                char? next = i + 1 < chars.Length ? (char?)chars[i + 1] : null;

                if (!char.IsLetterOrDigit(current))
                {
                    endWord();
                }
                else if ((char.IsDigit(current) && char.IsLetter(prev)) ||
                    (char.IsLetter(current) && char.IsDigit(prev)) ||
                    (char.IsUpper(current) && char.IsLower(prev)) ||
                    (char.IsUpper(current) && next != null && char.IsLower(next.Value)))
                {
                    endWord();
                    wordChars.Add(current);
                }
                else
                {
                    wordChars.Add(current);
                }
            }

            endWord();

            return words.ToArray();
        }
    }
}
