using System;
using System.Collections.Generic;
using _ = Atata.Tests.TriggersPage;

namespace Atata.Tests
{
    [Url("Triggers.html")]
    [VerifyTitle]
    [VerifyH1]
    [InvokeMethod(nameof(OnStaticInit), TriggerEvents.Init)]
    public class TriggersPage : Page<_>
    {
        [ThreadStatic]
        private static bool isOnInitInvoked;

        public TriggersPage()
        {
            isOnInitInvoked = false;

            Input.Triggers.Add(new WriteTriggerEventAttribute(TriggerEvents.Init));
        }

        public static bool IsOnInitInvoked => isOnInitInvoked;

        public bool IsBeforePerformInvoked { get; private set; }

        public bool IsAfterPerformInvoked { get; private set; }

        public List<TriggerEvents> InputEvents { get; private set; } = new List<TriggerEvents>();

        [InvokeMethod(nameof(OnBeforePerform), TriggerEvents.BeforeClick)]
        [InvokeMethod(nameof(OnAfterPerform), TriggerEvents.AfterClick)]
        public Button<_> Perform { get; private set; }

        [Term("Perform")]
        public Button<_> PerformWithoutTriggers { get; private set; }

        public Link<GoTo1Page, _> GoTo1 { get; private set; }

        public TextInput<_> Input { get; private set; }

        public static void OnStaticInit()
        {
            isOnInitInvoked = true;
        }

        public void OnBeforePerform()
        {
            IsBeforePerformInvoked = true;
        }

        public void OnAfterPerform()
        {
            IsAfterPerformInvoked = true;
        }

        public class WriteTriggerEventAttribute : SpecificTriggerAttribute
        {
            public WriteTriggerEventAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
                : base(on, priority)
            {
            }

            private void Execute(TriggerContext<_> context)
            {
                context.Component.Owner.InputEvents.Add(context.Event);
            }
        }
    }
}
