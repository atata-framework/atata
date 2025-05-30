namespace Atata.Xunit.IntegrationTests;

[CollectionDefinition(Name)]
public sealed class SomeCollection : ICollectionFixture<SomeCollectionFixture>
{
    public const string Name = "Some collection";
}
