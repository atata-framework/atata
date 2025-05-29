namespace Atata.Xunit3.IntegrationTests;

[CollectionDefinition(Name)]
[Trait("Xunit collection trait 1", "x")]
public sealed class SomeCollection : ICollectionFixture<SomeCollectionFixture>
{
    public const string Name = "Some collection";
}
