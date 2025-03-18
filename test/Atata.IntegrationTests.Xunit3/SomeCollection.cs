namespace Atata.IntegrationTests.Xunit3;

[CollectionDefinition(Name)]
public sealed class SomeCollection : ICollectionFixture<SomeCollectionFixture>
{
    public const string Name = "Some collection";
}
