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
            PropertyInfo property = destinationType.GetProperty(
                propertyName,
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            if (property == null)
                throw new MappingException($"Failed to map \"{propertyName}\" property for {destinationType.FullName} type. Property is not found.");

            Type propertyValueType = propertyValue?.GetType();
            Type propertyType = property.PropertyType;
            Type underlyingPropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            try
            {
                object valueToSet = propertyValue != null && !(propertyType.IsAssignableFrom(propertyValueType) || underlyingPropertyType.IsAssignableFrom(propertyValueType))
                    ? ConvertValue(propertyValue, underlyingPropertyType)
                    : propertyValue;

                property.SetValue(destination, valueToSet, null);
            }
            catch (Exception exception)
            {
                string additionalMessage = propertyValue == null
                    ? $"Property null value cannot be converted to {propertyType} type."
                    : $"Property \"{propertyValue}\" value of {propertyValueType} type cannot be converted to {propertyType} type.";

                throw new MappingException($"Failed to map \"{propertyName}\" property for {destinationType.FullName} type. {additionalMessage}", exception);
            }
        }

        private static object ConvertValue(object sourceValue, Type destinationType)
        {
            if (destinationType.IsEnum)
                return ConvertToEnum(destinationType, sourceValue);
            else if (destinationType == typeof(TimeSpan))
                return ConvertToTimeSpan(sourceValue);
            else
                return sourceValue;
        }

        private static object ConvertToEnum(Type enumType, object value)
        {
            if (value is string stringValue)
                return Enum.Parse(enumType, stringValue, true);
            else
                return Enum.ToObject(enumType, value);
        }

        private static TimeSpan ConvertToTimeSpan(object value)
        {
            if (value is double || value is int || value is float)
                return TimeSpan.FromSeconds(Convert.ToDouble(value));
            else
                return TimeSpan.Parse(value.ToString());
        }
    }
}
