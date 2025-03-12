#nullable enable

namespace Atata;

public interface IObjectMapper
{
    void Map(IEnumerable<KeyValuePair<string, object?>> propertiesMap, object destination);

    void Map(string propertyName, object? propertyValue, object destination);
}
