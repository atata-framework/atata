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

        [Test]
        public void Trigger_Add_ToControl()
        {
            page.PerformWithoutTriggers.Triggers.Add(new InvokeMethodAttribute(nameof(TriggersPage.OnBeforePerform), TriggerEvents.BeforeClick));

            page.PerformWithoutTriggers.Click();

            Assert.That(page.IsBeforePerformInvoked, Is.True);
            Assert.That(page.IsAfterPerformInvoked, Is.False);
        }

        [Test]
        public void Trigger_Add_ToPageObject()
        {
            bool isDeInitInvoked = false;

            page.Triggers.Add(new InvokeDelegateAttribute(() => isDeInitInvoked = true, TriggerEvents.DeInit));

            page.GoTo1.ClickAndGo();

            Assert.That(isDeInitInvoked, Is.True);
        }
    }
}
