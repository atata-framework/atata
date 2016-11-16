using NUnit.Framework;

namespace Atata.Tests
{
    public class TriggerTest : AutoTest
    {
        private TriggersPage page;

        protected override void OnSetUp()
        {
            page = Go.To<TriggersPage>();
        }

        [Test]
        public void Trigger_InvokeMethod_AtProperty()
        {
            page.Perform.Click();

            Assert.That(page.IsBeforePerformInvoked, Is.True);
            Assert.That(page.IsAfterPerformInvoked, Is.True);
        }

        [Test]
        public void Trigger_InvokeMethod_AtComponent()
        {
            Assert.That(TriggersPage.IsOnInitInvoked, Is.True);
        }
    }
}
