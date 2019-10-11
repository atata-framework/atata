using System.Linq;
using NUnit.Framework;

namespace Atata.Tests
{
    public class TriggerTests : UITestFixture
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
        public void Trigger_Add_ToDynamicControl()
        {
            page.DynamicInput.Triggers.Add(new LogInfoAttribute("AfterGet-Lowest", TriggerEvents.AfterGet, TriggerPriority.Lowest));

            page.DynamicInput.Get();

            VerifyLastLogMessages(
                "AfterGet-Medium",
                "AfterGet-Low",
                "AfterGet-Lower",
                "AfterGet-Lowest");
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

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            page.Input.Get(out string value);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
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

        [Test]
        public void Trigger_Priority()
        {
            VerifyLastLogMessages(
                "Init-Low",
                "Init-Lower",
                "Init-Lowest");

            page.InputWithLogging.Set("abc");

            VerifyLastLogMessages(
                "AfterSet-Highest",
                "AfterSet-Higher",
                "AfterSet-High",
                "AfterSet-Medium",
                "AfterSet-Low",
                "AfterSet-Lower",
                "AfterSet-Lowest");
        }

        [Test]
        public void Trigger_Get()
        {
            Assert.That(page.InputWithLogging.Triggers.AllTriggers.Count(), Is.EqualTo(9));
            Assert.That(page.InputWithLogging.Triggers.AllTriggers, Is.Ordered.By(nameof(TriggerAttribute.Priority)));

            Assert.That(page.InputWithLogging.Triggers.ParentComponentTriggers.Count(), Is.EqualTo(2));
            Assert.That(page.InputWithLogging.Triggers.DeclaredTriggers.Count(), Is.EqualTo(7));
        }

        [Test]
        public void Trigger_Remove()
        {
            bool isRemoved = page.InputWithLogging.Triggers.Remove(
                page.InputWithLogging.Triggers.DeclaredTriggers.OfType<LogInfoAttribute>().Single(x => x.Message == "AfterSet-Low"));

            Assert.That(isRemoved);
            Assert.That(page.InputWithLogging.Triggers.DeclaredTriggers.Count(), Is.EqualTo(6));

            page.InputWithLogging.Set("abc");

            VerifyLastLogMessages(
                "AfterSet-Highest",
                "AfterSet-Higher",
                "AfterSet-High",
                "AfterSet-Medium",
                "AfterSet-Lower",
                "AfterSet-Lowest");
        }

        [Test]
        public void Trigger_RemoveAll()
        {
            int countOfRemoved = page.InputWithLogging.Triggers.RemoveAll(
                x => x is TriggersPage.CustomLogInfoAttribute);

            Assert.That(countOfRemoved, Is.EqualTo(3));
            Assert.That(page.InputWithLogging.Triggers.DeclaredTriggers.Count(), Is.EqualTo(4));

            page.InputWithLogging.Set("abc");

            VerifyLastLogMessages(
                "AfterSet-Highest",
                "AfterSet-High",
                "AfterSet-Low",
                "AfterSet-Lowest");
        }

        [Test]
        public void Trigger_ChainExecution()
        {
            page.Hierarchy.Level1.Level2.Level3.Level4.Click();

            VerifyLastLogMessagesContain(
                "Starting: Click \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item's \"Level 3\" item's \"Level 4\" item",
                "Starting: Hover on \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item's \"Level 3\" item",
                "Starting: Hover on \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item",
                "Starting: Hover on \"Hierarchy\" control's \"Level 1\" item",
                "Finished: Hover on \"Hierarchy\" control's \"Level 1\" item",
                "Finished: Hover on \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item",
                "Finished: Hover on \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item's \"Level 3\" item",
                "Finished: Click \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item's \"Level 3\" item's \"Level 4\" item");
        }
    }
}
