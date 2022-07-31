using NUnit.Framework;

namespace Atata.IntegrationTests
{
    public class CheckBoxTests : UITestFixture
    {
        private CheckBoxListPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<CheckBoxListPage>();
        }

        [Test]
        public void CheckBox()
        {
            _page.
                OptionA.Should.BeFalse().
                OptionA.Should.Not.BeChecked().
                OptionA.Set(true).
                OptionA.Should.BeTrue().
                OptionA.Should.BeChecked().
                OptionA.Uncheck().
                OptionA.Should.Not.BeTrue().
                OptionA.Should.BeUnchecked().
                OptionA.Check().
                OptionA.Should.BeChecked();
        }
    }
}
