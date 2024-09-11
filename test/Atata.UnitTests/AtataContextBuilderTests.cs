#warning Review AtataContextBuilderTests.

////namespace Atata.UnitTests;

////[TestFixture]
////public class AtataContextBuilderTests
////{
////    private const string BaseUrl = "http://testapp.com/";

////    [Test]
////    [Parallelizable(ParallelScope.None)]
////    public void MixedConfiguration()
////    {
////        var globalContext = AtataContext.GlobalConfiguration.BuildingContext;

////        Assert.That(globalContext.TestNameFactory, Is.Null);
////        Assert.That(globalContext.DriverFactories, Is.Empty);
////        Assert.That(globalContext.LogConsumerConfigurations, Is.Empty);
////        Assert.That(globalContext.BaseUrl, Is.Null);

////        AtataContext.GlobalConfiguration
////            .UseNUnitTestName()
////            .LogConsumers.AddNUnitTestContext()
////            .EventSubscriptions.LogNUnitError();

////        var currentContext = AtataContext.Configure()
////            .UseEdge()
////            .UseBaseUrl(BaseUrl)
////            .EventSubscriptions.TakeScreenshotOnNUnitError()
////            .UseBaseRetryTimeout(TimeSpan.FromSeconds(100))
////            .UseBaseRetryInterval(TimeSpan.FromSeconds(1))
////            .BuildingContext;

////        AtataContext.GlobalConfiguration.Clear();

////        Assert.That(globalContext.TestNameFactory(), Is.EqualTo(nameof(MixedConfiguration)));
////        Assert.That(globalContext.DriverFactories, Is.Empty);
////        Assert.That(globalContext.LogConsumerConfigurations, Has.Count.EqualTo(1));
////        Assert.That(globalContext.LogConsumerConfigurations[0].Consumer, Is.TypeOf<NUnitTestContextLogConsumer>());
////        Assert.That(globalContext.BaseUrl, Is.Null);

////        Assert.That(currentContext.DriverFactories, Has.Count.EqualTo(1));
////        Assert.That(currentContext.DriverFactoryToUse.Alias, Is.EqualTo(WebDriverAliases.Edge));
////        Assert.That(currentContext.LogConsumerConfigurations, Has.Count.EqualTo(1));
////        Assert.That(currentContext.BaseUrl, Is.EqualTo(BaseUrl));
////        Assert.That(currentContext.BaseRetryTimeout, Is.EqualTo(TimeSpan.FromSeconds(100)));
////        Assert.That(currentContext.BaseRetryInterval, Is.EqualTo(TimeSpan.FromSeconds(1)));
////    }

////    [Test]
////    public void MultiDriverConfiguration()
////    {
////        var contextBuilder = AtataContext.Configure()
////            .UseEdge()
////            .UseChrome();

////        var context = contextBuilder.BuildingContext;

////        Assert.That(context.DriverFactories, Has.Count.EqualTo(2));
////        Assert.That(context.DriverFactoryToUse.Alias, Is.EqualTo(WebDriverAliases.Chrome));

////        contextBuilder.UseFirefox();

////        Assert.That(context.DriverFactories, Has.Count.EqualTo(3));
////        Assert.That(context.DriverFactoryToUse.Alias, Is.EqualTo(WebDriverAliases.Firefox));

////        contextBuilder.UseDriver(WebDriverAliases.Edge);

////        Assert.That(context.DriverFactories, Has.Count.EqualTo(3));
////        Assert.That(context.DriverFactoryToUse.Alias, Is.EqualTo(WebDriverAliases.Edge));

////        contextBuilder.UseDriver(WebDriverAliases.InternetExplorer);

////        Assert.That(context.DriverFactories, Has.Count.EqualTo(4));
////        Assert.That(context.DriverFactoryToUse.Alias, Is.EqualTo(WebDriverAliases.InternetExplorer));
////    }

////    [Test]
////    public void Clear()
////    {
////        var context = AtataContext.Configure()
////            .UseInternetExplorer()
////            .UseBaseUrl(BaseUrl)
////            .EventSubscriptions.TakeScreenshotOnNUnitError()
////            .PageSnapshots.UseCdpStrategy()
////            .UseBaseRetryTimeout(TimeSpan.FromSeconds(100))
////            .UseBaseRetryInterval(TimeSpan.FromSeconds(1))
////            .Clear()
////            .BuildingContext;

////        Assert.That(context.TestNameFactory, Is.Null);
////        Assert.That(context.DriverFactories, Is.Empty);
////        Assert.That(context.DriverFactoryToUse, Is.Null);
////        Assert.That(context.LogConsumerConfigurations, Is.Empty);
////        Assert.That(context.PageSnapshots.Strategy, Is.EqualTo(CdpOrPageSourcePageSnapshotStrategy.Instance));
////        Assert.That(context.BaseUrl, Is.Null);
////        Assert.That(context.BaseRetryTimeout, Is.EqualTo(TimeSpan.FromSeconds(5)));
////        Assert.That(context.BaseRetryInterval, Is.EqualTo(TimeSpan.FromSeconds(0.5)));
////    }
////}
