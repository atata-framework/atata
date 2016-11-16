using System;
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
        }

        public static bool IsOnInitInvoked => isOnInitInvoked;

        public bool IsBeforePerformInvoked { get; private set; }

        public bool IsAfterPerformInvoked { get; private set; }

        [InvokeMethod(nameof(OnBeforePerform), TriggerEvents.BeforeClick)]
        [InvokeMethod(nameof(OnAfterPerform), TriggerEvents.AfterClick)]
        public Button<_> Perform { get; private set; }

        private static void OnStaticInit()
        {
            isOnInitInvoked = true;
        }

        private void OnBeforePerform()
        {
            IsBeforePerformInvoked = true;
        }

        private void OnAfterPerform()
        {
            IsAfterPerformInvoked = true;
        }
    }
}
