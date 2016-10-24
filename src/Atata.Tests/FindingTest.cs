using NUnit.Framework;

namespace Atata.Tests
{
    public class FindingTest : AutoTest
    {
        private FindingPage page;

        protected override void OnSetUp()
        {
            page = Go.To<FindingPage>();
        }

        [Test]
        public void Find_ByNameAndIndex()
        {
            Assert.That(page.OptionC.Attributes["value"], Is.EqualTo("OptionC"));
        }
    }
}
