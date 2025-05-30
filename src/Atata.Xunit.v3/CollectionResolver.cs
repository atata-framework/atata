namespace Atata.Xunit;

internal static class CollectionResolver
{
    internal static bool TryResolveCollectionName(Type type, [NotNullWhen(true)] out string? collectionName)
    {
        var collectionAttribute = type.GetCustomAttributes(true)
            .OfType<ICollectionAttribute>()
            .FirstOrDefault();

        if (collectionAttribute is not null)
        {
            collectionName = collectionAttribute.Name;

            return collectionName is not null;
        }
        else
        {
            collectionName = null;
            return false;
        }
    }
}
