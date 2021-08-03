using System.Linq;
using NUnit.Framework;

namespace Atata.Tests
{
    public class TriggerTests : UITestFixture
    {
        private TriggersPage _page;

        protected override void OnSetUp()
        {
            _page = Go.To<TriggersPage>();
        }

        [Test]
        public void Trigger_InvokeMethod_AtProperty()
        {
            _page.Perform.Click();

            Assert.That(_page.IsBeforePerformInvoked, Is.True);
            Assert.That(_page.IsAfterPerformInvoked, Is.True);
        }

        [Test]
        public void Trigger_InvokeMethod_AtComponent()
        {
            Assert.That(TriggersPage.IsOnInitInvoked, Is.True);
        }

        [Test]
        public void Trigger_Add_ToControl()
        {
            _page.PerformWithoutTriggers.Metadata.Add(new InvokeMethodAttribute(nameof(TriggersPage.OnBeforePerform), TriggerEvents.BeforeClick));

            _page.PerformWithoutTriggers.Click();

            Assert.That(_page.IsBeforePerformInvoked, Is.True);
            Assert.That(_page.IsAfterPerformInvoked, Is.False);
        }

        [Test]
        public void Trigger_Add_ToDynamicControl()
        {
            _page.DynamicInput.Metadata.Add(new LogInfoAttribute("AfterGet-Lowest", TriggerEvents.AfterGet, TriggerPriority.Lowest));

            _page.DynamicInput.Get();

            VerifyLastLogMessages(
                minLogLevel: LogLevel.Info,
                "AfterGet-Medium",
                "AfterGet-Low",
                "AfterGet-Lower",
                "AfterGet-Lowest");
        }

        [Test]
        public void Trigger_Add_ToPageObject()
        {
            bool isDeInitInvoked = false;

            _page.Metadata.Add(new InvokeDelegateAttribute(() => isDeInitInvoked = true, TriggerEvents.DeInit));

            _page.GoTo1.ClickAndGo();

            Assert.That(isDeInitInvoked, Is.True);
        }

        [Test]
        public void Trigger_Events()
        {
            VerifyInputEvents(TriggerEvents.Init);

            _page.Input.Exists();
            VerifyInputEvents(TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess);

            _page.MissingInput.Missing();
            VerifyInputEvents(TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess);

            _page.Input.Should.Exist();
            VerifyInputEvents(TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess);

            _page.MissingInput.Should.Not.Exist();
            VerifyInputEvents(TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess);

            _page.Input.Attributes.Class.Should.HaveCount(1);
            VerifyInputEvents(TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess);

            _page.Input.Set("asd");
            VerifyInputEvents(TriggerEvents.BeforeSet, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterSet);

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            _page.Input.Get(out string value);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            VerifyInputEvents(TriggerEvents.BeforeGet, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterGet);

            _page.Input.Should.Equal("asd");
            VerifyInputEvents(TriggerEvents.BeforeGet, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterGet);

            _page.Input.Click();
            VerifyInputEvents(TriggerEvents.BeforeClick, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterClick);

            _page.Input.Hover();
            VerifyInputEvents(TriggerEvents.BeforeHover, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterHover);

            _page.Input.Focus();
            VerifyInputEvents(TriggerEvents.BeforeFocus, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterFocus);

            _page.Input.DoubleClick();
            VerifyInputEvents(TriggerEvents.BeforeClick, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterClick);

            _page.Input.RightClick();
            VerifyInputEvents(TriggerEvents.BeforeClick, TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess, TriggerEvents.AfterClick);

            _page.GoTo1.ClickAndGo();
            VerifyInputEvents(TriggerEvents.DeInit);
        }

        private void VerifyInputEvents(params TriggerEvents[] triggerEvents)
        {
            Assert.That(_page.InputEvents, Is.EqualTo(triggerEvents));
            _page.InputEvents.Clear();
        }

        [Test]
        public void Trigger_Priority()
        {
            VerifyLastLogMessages(
                minLogLevel: LogLevel.Info,
                "Init-Low",
                "Init-Lower",
                "Init-Lowest");

            _page.InputWithLogging.Set("abc");

            VerifyLastLogMessages(
                minLogLevel: LogLevel.Info,
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
            Assert.That(_page.InputWithLogging.Triggers.AllTriggers.Count(), Is.EqualTo(9));
            Assert.That(_page.InputWithLogging.Triggers.AllTriggers, Is.Ordered.By(nameof(TriggerAttribute.Priority)));

            Assert.That(_page.InputWithLogging.Triggers.ParentComponentTriggers.Count(), Is.EqualTo(2));
            Assert.That(_page.InputWithLogging.Triggers.DeclaredTriggers.Count(), Is.EqualTo(7));
        }

        [Test]
        public void Trigger_Remove()
        {
            bool isRemoved = _page.InputWithLogging.Metadata.Remove(
                _page.InputWithLogging.Triggers.DeclaredTriggers.OfType<LogInfoAttribute>().Single(x => x.Message == "AfterSet-Low"));

            Assert.That(isRemoved);
            Assert.That(_page.InputWithLogging.Triggers.DeclaredTriggers.Count(), Is.EqualTo(6));

            _page.InputWithLogging.Set("abc");

            VerifyLastLogMessages(
                minLogLevel: LogLevel.Info,
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
            int countOfRemoved = _page.InputWithLogging.Metadata.RemoveAll(
                x => x is TriggersPage.CustomLogInfoAttribute);

            Assert.That(countOfRemoved, Is.EqualTo(3));
            Assert.That(_page.InputWithLogging.Triggers.DeclaredTriggers.Count(), Is.EqualTo(4));

            _page.InputWithLogging.Set("abc");

            VerifyLastLogMessages(
                minLogLevel: LogLevel.Info,
                "AfterSet-Highest",
                "AfterSet-High",
                "AfterSet-Low",
                "AfterSet-Lowest");
        }

        [Test]
        public void Trigger_ChainExecution()
        {
            _page.Hierarchy.Level1.Level2.Level3.Level4.Click();

            VerifyLastLogMessagesContain(
                minLogLevel: LogLevel.Info,
                "> Click \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item's \"Level 3\" item's \"Level 4\" item",
                "> Hover on \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item's \"Level 3\" item",
                "> Hover on \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item",
                "> Hover on \"Hierarchy\" control's \"Level 1\" item",
                "< Hover on \"Hierarchy\" control's \"Level 1\" item",
                "< Hover on \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item",
                "< Hover on \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item's \"Level 3\" item",
                "< Click \"Hierarchy\" control's \"Level 1\" item's \"Level 2\" item's \"Level 3\" item's \"Level 4\" item");
        }
    }
}
