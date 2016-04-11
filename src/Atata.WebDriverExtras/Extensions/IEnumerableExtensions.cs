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
    }
}
