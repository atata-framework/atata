using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Atata.TermFormatting
{
    public class TitleTermFormatter : ITermFormatter
    {
        private static readonly string[] s_wordsToKeepLower = new[]
        {
            "a",
            "an",
            "the",
            "and",
            "but",
            "or",
            "for",
            "of",
            "nor",
            "on",
            "in",
            "at",
            "to",
            "from",
            "by",
            "to",
            "as",
            "with"
            ////"up",
            ////"down",
        };

        public string Format(string[] words)
        {
            List<string> updatedWords = new List<string>
            {
                CapitalizeFirstLetter(words[0])
            };

            if (words.Length > 2)
                updatedWords.AddRange(words.Skip(1).Take(words.Length - 2).Select(CapitalizeFirstLetterExceptSpecial));

            if (words.Length > 1)
                updatedWords.Add(CapitalizeFirstLetter(words[words.Length - 1]));

            return string.Join(" ", updatedWords);
        }

        private static string CapitalizeFirstLetter(string word)
        {
            return word.IsUpper() ? word : word.ToUpperFirstLetter();
        }

        private static string CapitalizeFirstLetterExceptSpecial(string word)
        {
            string wordToLower = word.ToLower(CultureInfo.CurrentCulture);
            return s_wordsToKeepLower.Contains(wordToLower) ? wordToLower : CapitalizeFirstLetter(word);
        }
    }
}
