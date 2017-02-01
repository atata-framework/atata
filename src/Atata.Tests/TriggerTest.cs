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

        [Test]
        public void Trigger_Events()
        {
            VerifyInputEvents(TriggerEvents.Init);

            page.Input.Exists();
            VerifyInputEvents(TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess);

            page.MissingInput.Missing();
            VerifyInputEvents(TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess);

            page.Input.Should.Exist();
            VerifyInputEvents(TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess);

            page.MissingInput.Should.Not.Exist();
            VerifyInputEvents(TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess);

            page.Input.Attributes.Class.Should.HaveCount(1);
            VerifyInputEvents(TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess);

            page.Input.Set("asd");
            VerifyInputEvents(TriggerEvents.BeforeSet, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterSet);

            string value;
            page.Input.Get(out value);
            VerifyInputEvents(TriggerEvents.BeforeGet, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterGet);

            page.Input.Should.Equal("asd");
            VerifyInputEvents(TriggerEvents.BeforeGet, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterGet);

            page.Input.Click();
            VerifyInputEvents(TriggerEvents.BeforeClick, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterClick);

            page.Input.Hover();
            VerifyInputEvents(TriggerEvents.BeforeHover, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterHover);

            page.Input.Focus();
            VerifyInputEvents(TriggerEvents.BeforeFocus, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterFocus);

            page.Input.DoubleClick();
            VerifyInputEvents(TriggerEvents.BeforeClick, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterClick);

            page.Input.RightClick();
            VerifyInputEvents(TriggerEvents.BeforeClick, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterClick);

            page.GoTo1.ClickAndGo();
            VerifyInputEvents(TriggerEvents.DeInit);
        }

        private void VerifyInputEvents(params TriggerEvents[] triggerEvents)
        {
            Assert.That(page.InputEvents, Is.EqualTo(triggerEvents));
            page.InputEvents.Clear();
        }
    }
}
