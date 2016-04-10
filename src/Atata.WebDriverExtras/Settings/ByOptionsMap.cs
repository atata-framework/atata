using OpenQA.Selenium;
using System.Collections.Generic;

namespace Atata
{
    public static class ByOptionsMap
    {
        // TODO: Add ReferenceEquals comparer.
        private static readonly Dictionary<By, ByOptions> AllOptions = new Dictionary<By, ByOptions>();

        internal static ByOptions GetAndStore(By by)
        {
            ByOptions options;
            return AllOptions.TryGetValue(by, out options) ? options : AllOptions[by] = ByOptions.CreateDefault();
        }

        internal static ByOptions GetOrDefault(By by)
        {
            ByOptions options;
            return AllOptions.TryGetValue(by, out options) ? options : ByOptions.CreateDefault();
        }

        internal static void Replace(By by, By newBy)
        {
            ByOptions options = AllOptions[by];
            AllOptions.Remove(by);
            AllOptions[newBy] = options;
        }
    }
}
