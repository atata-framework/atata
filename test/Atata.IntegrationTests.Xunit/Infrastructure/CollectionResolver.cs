using System.Reflection;
using Xunit;

namespace Atata.Xunit;

internal static class CollectionResolver
{
    internal static bool TryResolveCollectionName(Type type, out string collectionName)
    {
        var collectionAttribute = CustomAttributeData.GetCustomAttributes(type)
            .FirstOrDefault(x => x.AttributeType == typeof(CollectionAttribute));

        if (collectionAttribute is not null)
        {
            collectionName = (string)collectionAttribute.ConstructorArguments[0].Value;

            return collectionAttribute is not null;
        }
        else
        {
            collectionName = null;
            return false;
        }
    }
}
