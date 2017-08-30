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

            Type propertyValueType = propertyValue?.GetType();
            Type propertyType = property.PropertyType;
            Type underlyingPropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            object valueToSet = propertyValue != null && !(propertyType.IsAssignableFrom(propertyValueType) || underlyingPropertyType.IsAssignableFrom(propertyValueType))
                ? ConvertValue(propertyValue, underlyingPropertyType)
                : propertyValue;

            property.SetValue(destination, valueToSet, null);
        }

        private static object ConvertValue(object sourceValue, Type destinationType)
        {
            if (destinationType.IsEnum)
                return ConvertToEnum(destinationType, sourceValue);
            else
                return sourceValue;
        }

        private static object ConvertToEnum(Type enumType, object value)
        {
            if (value is string)
                return Enum.Parse(enumType, (string)value, true);
            else
                return Enum.ToObject(enumType, value);
        }
    }
}
