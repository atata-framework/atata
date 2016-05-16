using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class PageObjectTest : TestBase
    {
        [Test]
        public void PageObject_RefreshPage()
        {
            Go.To<BasicControlsPage>().
                ById.FirstName.Set("test").
                ById.FirstName.VerifyEquals("test").
                RefreshPage().
                ById.FirstName.VerifyIsNull();
        }
    }
}
