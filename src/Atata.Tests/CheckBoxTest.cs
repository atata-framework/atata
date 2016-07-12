using NUnit.Framework;

namespace Atata.Tests
{
    public class CheckBoxTest : TestBase
    {
        private CheckBoxListPage page;

        protected override void OnSetUp()
        {
            page = Go.To<CheckBoxListPage>();
        }

        [Test]
        public void CheckBox()
        {
            page.
                OptionA.Should.BeFalse().
                OptionA.Should.Not.BeChecked().
                OptionA.Set(true).
                OptionA.Should.BeTrue().
                OptionA.Should.BeChecked().
                OptionA.Uncheck().
                OptionA.Should.Not.BeTrue().
                OptionA.Should.Not.BeChecked().
                OptionA.Check().
                OptionA.Should.BeChecked();
        }
    }
}
