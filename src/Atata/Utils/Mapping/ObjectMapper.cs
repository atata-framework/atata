using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atata
{
    public class ObjectMapper : IObjectMapper
    {
        private readonly IObjectConverter objectConverter;

        public ObjectMapper(IObjectConverter objectConverter)
        {
            this.objectConverter = objectConverter;
        }

        public void Map(Dictionary<string, object> propertiesMap, object destination)
        {
            Type destinationType = destination.GetType();

            foreach (var item in propertiesMap)
            {
                Map(item.Key, item.Value, destination, destinationType);
            }
        }

        public void Map(string propertyName, object propertyValue, object destination)
        {
            Map(propertyName, propertyValue, destination, destination.GetType());
        }

        private void Map(string propertyName, object propertyValue, object destination, Type destinationType)
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
                object valueToSet = objectConverter.Convert(propertyValue, underlyingPropertyType);

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
    }
}
