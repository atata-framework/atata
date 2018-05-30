using System;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace Atata.Tests
{
    public class AtataContextBuilderTests : UITestFixtureBase
    {
        [Test]
        public void AtataContextBuilder_Build_WithoutDriver()
        {
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
               AtataContext.Configure().Build());

            Assert.That(exception.Message, Does.Contain("no driver is specified"));
        }

        [Test]
        public void AtataContextBuilder_OnBuilding()
        {
            int executionsCount = 0;

            ConfigureBaseAtataContext().
                OnBuilding(() =>
                {
                    Assert.That(AtataContext.Current.Log, Is.Not.Null);
                    Assert.That(AtataContext.Current.Driver, Is.Null);
                    executionsCount++;
                }).
                Build();

            Assert.That(executionsCount, Is.EqualTo(1));
        }

        [Test]
        public void AtataContextBuilder_OnBuilding_WithoutDriver()
        {
            int executionsCount = 0;

            Assert.Throws<InvalidOperationException>(() =>
                AtataContext.Configure().
                    UseDriver(() => null).
                    OnBuilding(() => executionsCount++).
                    Build());

            Assert.That(executionsCount, Is.EqualTo(1));
        }

        [Test]
        public void AtataContextBuilder_OnBuilt()
        {
            int executionsCount = 0;

            ConfigureBaseAtataContext().
                OnBuilt(() =>
                {
                    Assert.That(AtataContext.Current.Log, Is.Not.Null);
                    Assert.That(AtataContext.Current.Driver, Is.InstanceOf<ChromeDriver>());
                    executionsCount++;
                }).
                Build();

            Assert.That(executionsCount, Is.EqualTo(1));
        }

        [Test]
        public void AtataContextBuilder_OnDriverCreated()
        {
            int executionsCount = 0;

            ConfigureBaseAtataContext().
                OnDriverCreated(driver =>
                {
                    Assert.That(AtataContext.Current.Log, Is.Not.Null);
                    Assert.That(driver, Is.Not.Null.And.EqualTo(AtataContext.Current.Driver));
                    executionsCount++;
                }).
                Build();

            Assert.That(executionsCount, Is.EqualTo(1));
        }

        [Test]
        public void AtataContextBuilder_OnDriverCreated_RestartDriver()
        {
            int executionsCount = 0;

            ConfigureBaseAtataContext().
                OnDriverCreated(() => executionsCount++).
                Build();

            AtataContext.Current.RestartDriver();

            Assert.That(executionsCount, Is.EqualTo(2));
        }
    }
}
