using NUnit.Framework;

namespace Atata.Tests
{
    public class FindingWithSettingsTests : UITestFixture
    {
        private FindingWithSettingsPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<FindingWithSettingsPage>();
        }

        [Test]
        public void Find_WithSettings_AtPageObject()
        {
            _page.
                OptionA.Should.Not.Exist().
                OptionB.Should.Exist().
                OptionC.Should.Exist().
                OptionD.Should.Not.Exist();
        }

        [Test]
        public void Find_WithSettings_AtParentControl()
        {
            _page.
                RadioSet.OptionA.Should.Not.Exist().
                RadioSet.OptionB.Should.Exist().
                RadioSet.OptionC.Should.Not.Exist().
                RadioSet.OptionD.Should.Exist();
        }
    }
}
