using NUnit.Framework;

namespace Atata.Tests
{
    public class LabelListTests : UITestFixture
    {
        private LabelPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<LabelPage>();
        }

        [Test]
        public void LabelList()
        {
            _page.Labels[x => x.FirstName].Should.Equal("First Name");
            _page.Labels.For(x => x.LastName).Should.Equal("Last Name*");

            _page.Labels.Count.Should.BeGreater(1);
        }

        [Test]
        public void LabelList_MissingLabel()
        {
            _page.Labels[x => x.CompanyName].Should.Not.Exist();
        }
    }
}
