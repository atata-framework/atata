using NUnit.Framework;

namespace Atata.Tests
{
    public class RadioButtonTests : UITestFixture
    {
        private RadioButtonListPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<RadioButtonListPage>();
        }

        [Test]
        public void RadioButton()
        {
            _page.
                OptionA.Should.BeFalse().
                OptionA.Should.Not.BeChecked().
                OptionA.Check().
                OptionA.Should.BeTrue().
                OptionA.Should.BeChecked().
                OptionB.Check().
                OptionA.Should.Not.BeTrue().
                OptionA.Should.Not.BeChecked().
                OptionB.Should.Not.BeUnchecked().
                OptionA.Check().
                OptionA.Should.BeChecked();
        }
    }
}
