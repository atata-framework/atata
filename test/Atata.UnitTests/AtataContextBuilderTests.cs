namespace Atata.UnitTests;

[TestFixture]
public class AtataContextBuilderTests
{
    private const string BaseUrl = "http://testapp.com/";

    [Test]
    [Parallelizable(ParallelScope.None)]
    public void MixedConfiguration()
    {
        var globalContext = AtataContext.GlobalConfiguration.BuildingContext;

        using (new AssertionScope())
        {
            globalContext.TestNameFactory.Should().BeNull();
            globalContext.DriverFactories.Should().BeEmpty();
            globalContext.LogConsumerConfigurations.Should().BeEmpty();
            globalContext.BaseUrl.Should().BeNull();
        }

        AtataContext.GlobalConfiguration
            .UseNUnitTestName()
            .LogConsumers.AddNUnitTestContext()
            .EventSubscriptions.LogNUnitError();

        var currentContext = AtataContext.Configure()
            .UseEdge()
            .UseBaseUrl(BaseUrl)
            .EventSubscriptions.TakeScreenshotOnNUnitError()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(100))
            .UseBaseRetryInterval(TimeSpan.FromSeconds(1))
            .BuildingContext;

        AtataContext.GlobalConfiguration.Clear();

        using (new AssertionScope())
        {
            globalContext.TestNameFactory().Should().Be(nameof(MixedConfiguration));
            globalContext.DriverFactories.Should().BeEmpty();
            globalContext.LogConsumerConfigurations.Should().ContainSingle()
                .Which.Consumer.Should().BeOfType<NUnitTestContextLogConsumer>();
            globalContext.BaseUrl.Should().BeNull();

            currentContext.DriverFactories.Should().ContainSingle()
                .Which.Should().BeOfType<EdgeAtataContextBuilder>();
            currentContext.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Edge);
            currentContext.LogConsumerConfigurations.Should().ContainSingle()
                .Which.Consumer.Should().BeOfType<NUnitTestContextLogConsumer>();
            currentContext.BaseUrl.Should().Be(BaseUrl);
            currentContext.BaseRetryTimeout.Should().Be(TimeSpan.FromSeconds(100));
            currentContext.BaseRetryInterval.Should().Be(TimeSpan.FromSeconds(1));
        }
    }

    [Test]
    public void MultiDriverConfiguration()
    {
        var contextBuilder = AtataContext.Configure()
            .UseEdge()
            .UseChrome();

        var context = contextBuilder.BuildingContext;

        context.DriverFactories.Should().HaveCount(2);
        context.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Chrome);

        contextBuilder.UseFirefox();

        context.DriverFactories.Should().HaveCount(3);
        context.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Firefox);

        contextBuilder.UseDriver(DriverAliases.Edge);

        context.DriverFactories.Should().HaveCount(3);
        context.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Edge);

        contextBuilder.UseDriver(DriverAliases.InternetExplorer);

        context.DriverFactories.Should().HaveCount(4);
        context.DriverFactoryToUse.Alias.Should().Be(DriverAliases.InternetExplorer);
    }

    [Test]
    public void Clear()
    {
        var context = AtataContext.Configure()
            .UseInternetExplorer()
            .UseBaseUrl(BaseUrl)
            .EventSubscriptions.TakeScreenshotOnNUnitError()
            .PageSnapshots.UseCdpStrategy()
            .UseBaseRetryTimeout(TimeSpan.FromSeconds(100))
            .UseBaseRetryInterval(TimeSpan.FromSeconds(1))
            .Clear()
            .BuildingContext;

        using (new AssertionScope())
        {
            context.TestNameFactory.Should().BeNull();
            context.DriverFactories.Should().BeEmpty();
            context.DriverFactoryToUse.Should().BeNull();
            context.LogConsumerConfigurations.Should().BeEmpty();
            context.PageSnapshots.Strategy.Should().Be(CdpOrPageSourcePageSnapshotStrategy.Instance);
            context.BaseUrl.Should().BeNull();
            context.BaseRetryTimeout.Should().Be(TimeSpan.FromSeconds(5));
            context.BaseRetryInterval.Should().Be(TimeSpan.FromSeconds(0.5));
        }
    }
}
