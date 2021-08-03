using NUnit.Framework;

namespace Atata.Tests
{
    public class LabelTests : UITestFixture
    {
        private LabelPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<LabelPage>();
        }

        [Test]
        public void Label()
        {
            _page.FirstNameLabel.Should.Equal("First Name");
            _page.FirstNameLabel.Attributes.For.Should.Equal("first-name");

            _page.LastNameLabel.Should.Equal("Last Name");

            _page.LastNameByForLabel.Should.Equal("Last Name*");
        }
    }
}
