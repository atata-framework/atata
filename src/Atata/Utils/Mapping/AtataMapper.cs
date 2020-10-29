using System;
using System.Collections.Generic;

namespace Atata
{
    [Obsolete("Use AtataContext.Current.ObjectMapper instead.")] // Obsolete since v1.8.0.
    public static class AtataMapper
    {
        public static void Map(Dictionary<string, object> propertiesMap, object destination)
        {
            ResolveObjectManager().Map(propertiesMap, destination);
        }

        public static void Map(string propertyName, object propertyValue, object destination)
        {
            ResolveObjectManager().Map(propertyName, propertyValue, destination);
        }

        private static IObjectMapper ResolveObjectManager()
        {
            return AtataContext.Current?.ObjectMapper
                ?? new ObjectMapper(new ObjectConverter());
        }
    }
}
