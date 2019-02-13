using NUnit.Framework;

namespace Atata.Tests
{
    public class LabelListTests : UITestFixture
    {
        private LabelPage page;

        protected override void OnSetUp()
        {
            page = Go.To<LabelPage>();
        }

        [Test]
        public void LabelList()
        {
            page.Labels[x => x.FirstName].Should.Equal("First Name");
            page.Labels.For(x => x.LastName).Should.Equal("Last Name*");

            page.Labels.Count.Should.BeGreater(1);
        }

        [Test]
        public void LabelList_MissingLabel()
        {
            page.Labels[x => x.CompanyName].Should.Not.Exist();
        }
    }
}
