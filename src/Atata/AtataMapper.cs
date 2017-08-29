using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atata
{
    public static class AtataMapper
    {
        public static void Map(Dictionary<string, object> propertiesMap, object destination)
        {
            Type destinationType = destination.GetType();

            foreach (var item in propertiesMap)
            {
                Map(item.Key, item.Value, destination, destinationType);
            }
        }

        public static void Map(string propertyName, object propertyValue, object destination)
        {
            Map(propertyName, propertyValue, destination, destination.GetType());
        }

        private static void Map(string propertyName, object propertyValue, object destination, Type destinationType)
        {
            PropertyInfo property = destinationType.GetPropertyWithThrowOnError(
                propertyName,
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            property.SetValue(destination, propertyValue, null);
        }
    }
}
