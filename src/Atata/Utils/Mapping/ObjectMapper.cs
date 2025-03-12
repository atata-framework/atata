#nullable enable

namespace Atata;

public class ObjectMapper : IObjectMapper
{
    private readonly IObjectConverter _objectConverter;

    public ObjectMapper(IObjectConverter objectConverter) =>
        _objectConverter = objectConverter;

    public void Map(IEnumerable<KeyValuePair<string, object>> propertiesMap, object destination)
    {
        propertiesMap.CheckNotNull(nameof(propertiesMap));
        destination.CheckNotNull(nameof(destination));

        Type destinationType = destination.GetType();

        foreach (var item in propertiesMap)
        {
            Map(item.Key, item.Value, destination, destinationType);
        }
    }

    public void Map(string propertyName, object propertyValue, object destination)
    {
        destination.CheckNotNull(nameof(destination));

        Map(propertyName, propertyValue, destination, destination.GetType());
    }

    private void Map(string propertyName, object propertyValue, object destination, Type destinationType)
    {
        propertyName.CheckNotNullOrWhitespace(nameof(propertyName));

        PropertyInfo property = destinationType.GetProperty(
            propertyName,
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase)
            ?? throw new MappingException(
                BuildMappingExceptionMessage(destinationType, propertyName, "Property is not found."));

        if (!property.CanWrite)
            throw new MappingException(
                BuildMappingExceptionMessage(destinationType, property.Name, "Property cannot be set."));

        Type? propertyValueType = propertyValue?.GetType();
        Type propertyType = property.PropertyType;
        Type underlyingPropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        try
        {
            object? valueToSet = _objectConverter.Convert(propertyValue, underlyingPropertyType);

            property.SetValue(destination, valueToSet, null);
        }
        catch (Exception exception)
        {
            string additionalMessage = propertyValue is null
                ? $"Property null value cannot be converted to {propertyType.FullName} type."
                : $"Property \"{propertyValue}\" value of {propertyValueType!.FullName} type cannot be converted to {propertyType.FullName} type.";

            throw new MappingException(
                BuildMappingExceptionMessage(destinationType, property.Name, additionalMessage),
                exception);
        }
    }

    private static string BuildMappingExceptionMessage(Type type, string propertyName, string additionalMessage) =>
        $"Failed to map \"{propertyName}\" property for {type.FullName} type. {additionalMessage}";
}
