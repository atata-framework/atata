using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Atata.Tests
{
    public class AtataContextEventsTests : UITestFixtureBase
    {
        [Test]
        public void Init()
        {
            int executionsCount = 0;

            ConfigureBaseAtataContext()
                .EventSubscriptions.Add<AtataContextInitStartedEvent>((eventData, context) =>
                {
                    eventData.Should().NotBeNull();
                    context.Should().NotBeNull().And.Be(eventData.Context).And.Be(AtataContext.Current);

                    context.Log.Should().NotBeNull();
                    context.Driver.Should().BeNull();

                    executionsCount++;
                })
                .Build();

            executionsCount.Should().Be(1);
        }

        [Test]
        public void Init_WithNullDriver()
        {
            int executionsCount = 0;

            Assert.Throws<InvalidOperationException>(() =>
                AtataContext.Configure()
                    .UseDriver(() => null)
                    .EventSubscriptions.Add<AtataContextInitStartedEvent>(() => executionsCount++)
                    .Build());

            executionsCount.Should().Be(1);
        }

        [Test]
        public void InitCompleted()
        {
            int executionsCount = 0;

            ConfigureBaseAtataContext()
                .EventSubscriptions.Add<AtataContextInitCompletedEvent>((eventData, context) =>
                {
                    eventData.Should().NotBeNull();
                    context.Should().NotBeNull().And.Be(eventData.Context).And.Be(AtataContext.Current);

                    context.Log.Should().NotBeNull();
                    context.Driver.Should().BeOfType<ChromeDriver>();

                    executionsCount++;
                })
                .Build();

            executionsCount.Should().Be(1);
        }

        [Test]
        public void DriverInit()
        {
            int executionsCount = 0;

            ConfigureBaseAtataContext()
                .EventSubscriptions.Add<DriverInitEvent>((eventData, context) =>
                {
                    eventData.Should().NotBeNull();
                    context.Should().NotBeNull().And.Be(AtataContext.Current);

                    context.Log.Should().NotBeNull();
                    eventData.Driver.Should().NotBeNull().And.Be(AtataContext.Current.Driver);

                    executionsCount++;
                })
                .Build();

            executionsCount.Should().Be(1);
        }

        [Test]
        public void DriverInit_WhenRestartDriver()
        {
            int executionsCount = 0;
            IWebDriver initialDriver = null;

            ConfigureBaseAtataContext()
                .EventSubscriptions.Add<DriverInitEvent>((eventData, context) =>
                {
                    if (executionsCount == 0)
                        initialDriver = eventData.Driver;
                    else
                        eventData.Driver.Should().NotBe(initialDriver);

                    executionsCount++;
                })
                .Build();

            AtataContext.Current.RestartDriver();

            executionsCount.Should().Be(2);
        }
    }
}
