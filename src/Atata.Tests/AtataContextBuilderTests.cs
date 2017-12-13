using System;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class AtataContextBuilderTests
    {
        [Test]
        public void AtataContextBuilder_Build_WithoutDriver()
        {
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
               AtataContext.Configure().Build());

            Assert.That(exception.Message, Does.Contain("no driver is specified"));
        }
    }
}
