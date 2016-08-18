using NUnit.Framework;

namespace Atata.Tests
{
    public class RadioButtonTest : AutoTest
    {
        private RadioButtonListPage page;

        protected override void OnSetUp()
        {
            page = Go.To<RadioButtonListPage>();
        }

        [Test]
        public void RadioButton()
        {
            page.
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
