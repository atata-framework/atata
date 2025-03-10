namespace Atata.UnitTests.WebDriver;

public sealed class WebDriverSessionBuilderTests
{
    [Test]
    public void Clone()
    {
        // Arrange
        var original = WebDriverSession.CreateBuilder();
        original.UseDriver(WebDriverAliases.Edge);
        original.UseDriver(WebDriverAliases.Chrome);

        // Act
        var copy = original.Clone();

        // Assert
        copy.DriverFactories.Should().HaveCount(2);
        copy.DriverFactories.Should().NotContain(original.DriverFactories);

        copy.DriverFactoryToUse.Alias.Should().Be(WebDriverAliases.Chrome);
        copy.DriverFactoryToUse.Should().NotBe(original.DriverFactoryToUse);
        copy.DriverFactories.Should().Contain(copy.DriverFactoryToUse);
    }

    public sealed class UseDriver_WithAlias
    {
        [Test]
        public void WhenThereAreNoDrivers()
        {
            // Arrange
            var builder = WebDriverSession.CreateBuilder();

            // Act
            builder.UseDriver(WebDriverAliases.Edge);

            // Assert
            builder.DriverFactories.Should().HaveCount(1);
            builder.DriverFactoryToUse.Alias.Should().Be(WebDriverAliases.Edge);
        }

        [Test]
        public void WhenThereAreNoDrivers_CallTwice()
        {
            // Arrange
            var builder = WebDriverSession.CreateBuilder();

            // Act
            builder.UseDriver(WebDriverAliases.Edge);
            builder.UseDriver(WebDriverAliases.Edge);

            // Assert
            builder.DriverFactories.Should().HaveCount(1);
            builder.DriverFactoryToUse.Alias.Should().Be(WebDriverAliases.Edge);
        }

        [Test]
        public void WhenThereIsSuchDriver()
        {
            // Arrange
            var builder = WebDriverSession.CreateBuilder();
            builder.UseEdge();

            // Act
            builder.UseDriver(WebDriverAliases.Edge);

            // Assert
            builder.DriverFactories.Should().HaveCount(1);
            builder.DriverFactoryToUse.Alias.Should().Be(WebDriverAliases.Edge);
        }
    }

    public sealed class UseDriver_WithBuilder
    {
        [Test]
        public void WhenThereAreNoDrivers()
        {
            // Arrange
            var builder = WebDriverSession.CreateBuilder();

            // Act
            builder.UseEdge();

            // Assert
            builder.DriverFactories.Should().HaveCount(1);
            builder.DriverFactoryToUse.Alias.Should().Be(WebDriverAliases.Edge);
        }

        [Test]
        public void WhenThereIsSuchDriver()
        {
            // Arrange
            var builder = WebDriverSession.CreateBuilder();
            builder.UseEdge();

            // Act
            builder.UseEdge();

            // Assert
            builder.DriverFactories.Should().HaveCount(2);
        }

        [Test]
        public void WhenThereIsAnotherDriver()
        {
            // Arrange
            var builder = WebDriverSession.CreateBuilder();
            builder.UseEdge();

            // Act
            builder.UseChrome();

            // Assert
            builder.DriverFactories.Should().HaveCount(2);
            builder.DriverFactoryToUse.Alias.Should().Be(WebDriverAliases.Chrome);
        }
    }
}
