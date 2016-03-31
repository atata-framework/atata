using Humanizer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Atata
{
    public static class IEnumerableExtensions
    {
        public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> source)
        {
            return new ReadOnlyCollection<T>(source.ToList());
        }

        private static ReadOnlyCollection<T> NullIfEmpty<T>(this ReadOnlyCollection<T> source)
        {
            return source.Any() ? source : null;
        }

        public static string ToQuotedValuesListOfString(this IEnumerable<string> source, bool doubleQuotes = false)
        {
            char quotesCharacter = doubleQuotes ? '"' : '\'';
            string separator = "{0}, {0}".FormatWith(quotesCharacter);
            return "{0}{1}{0}".FormatWith(quotesCharacter, string.Join(separator, source));
        }
    }
}
