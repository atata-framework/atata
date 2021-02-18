using NUnit.Framework;

namespace Atata.Tests
{
    public class FindingWithSettingsTests : UITestFixture
    {
        private FindingWithSettingsPage page;

        protected override void OnSetUp()
        {
            page = Go.To<FindingWithSettingsPage>();
        }

        [Test]
        public void Find_WithSettings_AtPageObject()
        {
            page.
                OptionA.Should.Not.Exist().
                OptionB.Should.Exist().
                OptionC.Should.Exist().
                OptionD.Should.Not.Exist();
        }

        [Test]
        public void Find_WithSettings_AtParentControl()
        {
            page.
                RadioSet.OptionA.Should.Not.Exist().
                RadioSet.OptionB.Should.Exist().
                RadioSet.OptionC.Should.Not.Exist().
                RadioSet.OptionD.Should.Exist();
        }
    }
}
