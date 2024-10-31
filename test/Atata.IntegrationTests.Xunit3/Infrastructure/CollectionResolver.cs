using Xunit.v3;

namespace Atata.Xunit;

internal static class CollectionResolver
{
    internal static bool TryResolveCollectionName(Type type, out string collectionName)
    {
        var collectionAttribute = type.GetCustomAttributes(true)
            .OfType<ICollectionAttribute>()
            .FirstOrDefault();

        if (collectionAttribute is not null)
        {
            collectionName = collectionAttribute.Name;

            return collectionAttribute is not null;
        }
        else
        {
            collectionName = null;
            return false;
        }
    }
}
