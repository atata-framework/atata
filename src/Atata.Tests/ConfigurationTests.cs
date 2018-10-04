using System;
using System.Linq;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        [Parallelizable(ParallelScope.None)]
        public void Configuration_Mixed()
        {
            var globalContext = AtataContext.GlobalConfiguration.BuildingContext;

            Assert.That(globalContext.TestNameFactory, Is.Null);
            Assert.That(globalContext.DriverFactories, Is.Empty);
            Assert.That(globalContext.LogConsumers, Is.Empty);
            Assert.That(globalContext.ScreenshotConsumers, Is.Empty);
            Assert.That(globalContext.BaseUrl, Is.Null);
            Assert.That(globalContext.CleanUpActions, Is.Empty);

            AtataContext.GlobalConfiguration.
                UseNUnitTestName().
                AddNUnitTestContextLogging().
                LogNUnitError();

            var currentContext = AtataContext.Configure().
                UsePhantomJS().
                UseBaseUrl(UITestFixtureBase.BaseUrl).
                TakeScreenshotOnNUnitError().
                AddScreenshotFileSaving().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(100)).
                UseBaseRetryInterval(TimeSpan.FromSeconds(1)).
                BuildingContext;

            AtataContext.GlobalConfiguration.Clear();

            Assert.That(globalContext.TestNameFactory(), Is.EqualTo(nameof(Configuration_Mixed)));
            Assert.That(globalContext.DriverFactories, Is.Empty);
            Assert.That(globalContext.LogConsumers, Has.Count.EqualTo(1));
            Assert.That(globalContext.LogConsumers.First().Consumer, Is.TypeOf<NUnitTestContextLogConsumer>());
            Assert.That(globalContext.ScreenshotConsumers, Is.Empty);
            Assert.That(globalContext.BaseUrl, Is.Null);
            Assert.That(globalContext.CleanUpActions, Has.Count.EqualTo(1));

            Assert.That(currentContext.DriverFactories, Has.Count.EqualTo(1));
            Assert.That(currentContext.DriverFactoryToUse.Alias, Is.EqualTo(DriverAliases.PhantomJS));
            Assert.That(currentContext.LogConsumers, Has.Count.EqualTo(1));
            Assert.That(currentContext.ScreenshotConsumers, Has.Count.EqualTo(1));
            Assert.That(currentContext.ScreenshotConsumers.First(), Is.TypeOf<FileScreenshotConsumer>());
            Assert.That(currentContext.BaseUrl, Is.EqualTo(UITestFixtureBase.BaseUrl));
            Assert.That(currentContext.CleanUpActions, Has.Count.EqualTo(2));
            Assert.That(currentContext.BaseRetryTimeout, Is.EqualTo(TimeSpan.FromSeconds(100)));
            Assert.That(currentContext.BaseRetryInterval, Is.EqualTo(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void Configuration_MultiDriver()
        {
            var contextBuilder = AtataContext.Configure().
                UsePhantomJS().
                UseChrome();

            var context = contextBuilder.BuildingContext;

            Assert.That(context.DriverFactories, Has.Count.EqualTo(2));
            Assert.That(context.DriverFactoryToUse.Alias, Is.EqualTo(DriverAliases.Chrome));

            contextBuilder.
                UseFirefox();

            Assert.That(context.DriverFactories, Has.Count.EqualTo(3));
            Assert.That(context.DriverFactoryToUse.Alias, Is.EqualTo(DriverAliases.Firefox));

            contextBuilder.
                UseDriver(DriverAliases.PhantomJS);

            Assert.That(context.DriverFactories, Has.Count.EqualTo(3));
            Assert.That(context.DriverFactoryToUse.Alias, Is.EqualTo(DriverAliases.PhantomJS));

            contextBuilder.
                UseDriver(DriverAliases.InternetExplorer);

            Assert.That(context.DriverFactories, Has.Count.EqualTo(4));
            Assert.That(context.DriverFactoryToUse.Alias, Is.EqualTo(DriverAliases.InternetExplorer));
        }

        [Test]
        public void Configuration_Clear()
        {
            var context = AtataContext.Configure().
                UsePhantomJS().
                UseBaseUrl(UITestFixtureBase.BaseUrl).
                TakeScreenshotOnNUnitError().
                AddScreenshotFileSaving().
                UseBaseRetryTimeout(TimeSpan.FromSeconds(100)).
                UseBaseRetryInterval(TimeSpan.FromSeconds(1)).
                Clear().
                BuildingContext;

            Assert.That(context.TestNameFactory, Is.Null);
            Assert.That(context.DriverFactories, Is.Empty);
            Assert.That(context.DriverFactoryToUse, Is.Null);
            Assert.That(context.LogConsumers, Is.Empty);
            Assert.That(context.ScreenshotConsumers, Is.Empty);
            Assert.That(context.BaseUrl, Is.Null);
            Assert.That(context.CleanUpActions, Is.Empty);
            Assert.That(context.BaseRetryTimeout, Is.EqualTo(TimeSpan.FromSeconds(5)));
            Assert.That(context.BaseRetryInterval, Is.EqualTo(TimeSpan.FromSeconds(0.5)));
        }
    }
}
