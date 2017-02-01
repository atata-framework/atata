using NUnit.Framework;

namespace Atata.Tests
{
    public class LabelTest : AutoTest
    {
        private LabelPage page;

        protected override void OnSetUp()
        {
            page = Go.To<LabelPage>();
        }

        [Test]
        public void Label()
        {
            page.FirstNameLabel.Should.Equal("First Name");
            page.FirstNameLabel.Attributes.For.Should.Equal("first-name");

            page.LastNameLabel.Should.Equal("Last Name");

            page.LastNameByForLabel.Should.Equal("Last Name*");
        }
    }
}
