namespace Atata.Xunit;

internal static class CollectionResolver
{
    internal static bool TryResolveCollectionName(Type type, [NotNullWhen(true)] out string? collectionName)
    {
        var collectionAttribute = CustomAttributeData.GetCustomAttributes(type)
            .FirstOrDefault(x => x.AttributeType == typeof(CollectionAttribute));

        if (collectionAttribute is not null)
        {
            collectionName = collectionAttribute.ConstructorArguments[0].Value as string;

            return collectionName is not null;
        }
        else
        {
            collectionName = null;
            return false;
        }
    }
}
