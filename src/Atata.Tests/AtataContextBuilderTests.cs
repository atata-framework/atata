using System;
using NUnit.Framework;

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

            AtataContext.Configure().
                UseChrome().
                OnBuilding(() => executionsCount++).
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
        public void AtataContextBuilder_OnDriverCreated()
        {
            int executionsCount = 0;

            AtataContext.Configure().
                UseChrome().
                OnDriverCreated(driver => executionsCount++).
                Build();

            Assert.That(executionsCount, Is.EqualTo(1));
        }

        [Test]
        public void AtataContextBuilder_OnDriverCreated_RestartDriver()
        {
            int executionsCount = 0;

            AtataContext.Configure().
                UseChrome().
                OnDriverCreated(() => executionsCount++).
                Build();

            AtataContext.Current.RestartDriver();

            Assert.That(executionsCount, Is.EqualTo(2));
        }
    }
}
