using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public static class TermFormatExtensions
    {
        public static string ApplyTo(this TermFormat format, string value)
        {
            switch (format)
            {
                case TermFormat.None:
                    return value;
                case TermFormat.Title:
                    return ToTitle(SplitIntoWords(value));
                case TermFormat.TitleWithColon:
                    return value.Humanize(LetterCasing.Title) + ":";
                case TermFormat.Sentence:
                    return value.Humanize(LetterCasing.Sentence);
                case TermFormat.SentenceWithColon:
                    return value.Humanize(LetterCasing.Sentence) + ":";
                case TermFormat.LowerCase:
                    return value.Humanize(LetterCasing.LowerCase);
                case TermFormat.UpperCase:
                    return value.Humanize(LetterCasing.AllCaps);
                case TermFormat.Camel:
                    return value.Camelize();
                case TermFormat.Pascal:
                    return value.Pascalize();
                case TermFormat.Dashed:
                    return value.Underscore().Dasherize();
                case TermFormat.Hyphenated:
                    return value.Underscore().Hyphenate();
                case TermFormat.PascalDashed:
                    return value.Underscore().PascalDasherize();
                case TermFormat.PascalHyphenated:
                    return value.Underscore().PascalHyphenate();
                case TermFormat.XDashed:
                    return "x-" + value.Underscore().Dasherize();
                case TermFormat.Underscored:
                    return value.Underscore();
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(format, "format");
            }
        }

        private static string ToTitle(string[] words)
        {
            return string.Join(" ", words.Select(x => x.ToUpper() == x ? x : x.ToUpperFirstLetter()));
        }

        public static string[] SplitIntoWords(string value)
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
