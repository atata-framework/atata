using Xunit;

namespace Atata.IntegrationTests.Xunit;

[CollectionDefinition(Name)]
public sealed class SomeCollection : ICollectionFixture<SomeCollectionFixture>
{
    public const string Name = "Some collection";
}
