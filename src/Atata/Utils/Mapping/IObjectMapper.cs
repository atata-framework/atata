using System.Collections.Generic;

namespace Atata
{
    public interface IObjectMapper
    {
        void Map(Dictionary<string, object> propertiesMap, object destination);

        void Map(string propertyName, object propertyValue, object destination);
    }
}
