using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class GoToTest : TestBase
    {
        [Test]
        public void GoTo_SimpleNavigation()
        {
            Go.To<GoTo1Page>();
            Go.To<GoTo2Page>();
        }

        [Test]
        public void GoTo_UrlNavigation()
        {
            Go.To<GoTo1Page>();
            string url = "http://localhost:50549/GoTo2.html?somearg=1";
            Go.To<GoTo2Page>(url: url);

            Assert.That(AtataContext.Current.Driver.Url, Is.EqualTo(url));
        }
    }
}
