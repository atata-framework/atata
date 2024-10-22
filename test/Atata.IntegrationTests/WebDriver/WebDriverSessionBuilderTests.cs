using OpenQA.Selenium.Chrome;

namespace Atata.IntegrationTests.WebDriver;

public sealed partial class WebDriverSessionBuilderTests : WebDriverSessionTestSuiteBase
{
    [Test]
    public void Build_WithoutDriver()
    {
        var builder = AtataContext.CreateBuilder(AtataContextScope.Test);
        builder.Sessions.AddWebDriver();

        var exception = Assert.Throws<AtataSessionBuilderValidationException>(() =>
           builder.Build());

        exception.Message.Should().Contain("no driver is specified");
    }

    [Test]
    public void Build_WithNullDriver()
    {
        var builder = AtataContext.CreateBuilder(AtataContextScope.Test);
        builder.Sessions.AddWebDriver(x => x.UseDriver(() => null));

        var exception = Assert.Throws<WebDriverInitializationException>(
            () => builder.Build());

        exception.Message.Should().Be("Driver factory returned null as a driver.");
    }

    [Test]
    public void Build_WithoutUseDriverButWithConfigureChrome()
    {
        var context = AtataContext.CreateBuilder(AtataContextScope.Test)
            .Sessions.AddWebDriver(x => x
                .ConfigureChrome()
                    .WithArguments(ChromeArguments))
            .Build();

        context.GetWebDriverSession().Driver.Should().BeOfType<ChromeDriver>();
    }

    [Test]
    public void ConfigureChrome_After_UseChrome_ExtendsSameFactory()
    {
        var builder = AtataContext.CreateBuilder(AtataContextScope.Test)
            .Sessions.AddWebDriver(x =>
            {
                x.UseChrome();
                x.ConfigureChrome();
            });

        var sessionBuilder = builder.Sessions.Builders.OfType<WebDriverSessionBuilder>().Single();
        sessionBuilder.DriverFactories.Should().HaveCount(1);
        sessionBuilder.DriverFactoryToUse.Alias.Should().Be(WebDriverAliases.Chrome);
    }

    [Test]
    public void ConfigureChrome_After_UseChrome_CreatesNewFactory_WhenOtherAliasIsSpecified()
    {
        var builder = AtataContext.CreateBuilder(AtataContextScope.Test)
            .Sessions.AddWebDriver(x =>
            {
                x.UseChrome();
                x.ConfigureChrome("chrome_other");
            });

        var sessionBuilder = builder.Sessions.Builders.OfType<WebDriverSessionBuilder>().Single();
        sessionBuilder.DriverFactories.Should().HaveCount(2);
        sessionBuilder.DriverFactoryToUse.Alias.Should().Be(WebDriverAliases.Chrome);
        sessionBuilder.DriverFactories[1].Alias.Should().Be("chrome_other");
    }

    [Test]
    public void ConfigureChrome_After_UseFirefox_WithSameAlias_Throws() =>
        AtataContext.CreateBuilder(AtataContextScope.Test)
            .Sessions.AddWebDriver(x =>
            {
                x.UseFirefox()
                    .WithAlias("drv");

                Assert.Throws<ArgumentException>(() =>
                    x.ConfigureChrome("drv"));
            });

    [Test]
    public void ConfigureChrome_After_UseChrome_Executes()
    {
        bool isChromeConfigurationInvoked = false;

        AtataContext.CreateBuilder(AtataContextScope.Test)
            .Sessions.AddWebDriver(x =>
            {
                x.UseChrome()
                    .WithArguments(ChromeArguments);
                x.ConfigureChrome()
                    .WithOptions(_ => isChromeConfigurationInvoked = true);
            })
            .Build();

        isChromeConfigurationInvoked.Should().BeTrue();
    }

    [Test]
    public void UseDisposeDriver_WithTrue()
    {
        var context = BuildAtataContextWithWebDriverSession(
            x => x.UseDisposeDriver(true));
        var driver = context.GetWebDriver();

        context.Dispose();

        Assert.Throws<ObjectDisposedException>(() =>
            _ = driver.Url);
    }

    [Test]
    public void UseDisposeDriver_WithFalse()
    {
        var context = BuildAtataContextWithWebDriverSession(
            x => x.UseDisposeDriver(false));
        var driver = context.GetWebDriver();

        context.Dispose();

        Assert.DoesNotThrow(() =>
            _ = driver.Url);

        driver.Dispose();
    }
}
