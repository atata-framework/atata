using System;
using NUnit.Framework;

namespace Atata.Tests
{
    public class AtataContextWithoutDriverTests : UITestFixtureBase
    {
        [Test]
        public void When_DriverInitializationStage_None()
        {
            var sut = AtataContext.Configure()
                .UseDriverInitializationStage(AtataContextDriverInitializationStage.None);

            sut.Build();

            Assert.That(AtataContext.Current.Driver, Is.Null);
        }

        [Test]
        public void When_DriverInitializationStage_Build()
        {
            var sut = AtataContext.Configure()
                .UseDriverInitializationStage(AtataContextDriverInitializationStage.Build);

            Assert.Throws<InvalidOperationException>(() =>
                sut.Build());
        }

        [Test]
        public void When_DriverInitializationStage_OnDemand()
        {
            var sut = AtataContext.Configure()
                .UseDriverInitializationStage(AtataContextDriverInitializationStage.OnDemand);

            sut.Build();

            Assert.Throws<InvalidOperationException>(() =>
                _ = AtataContext.Current.Driver);
        }
    }
}
