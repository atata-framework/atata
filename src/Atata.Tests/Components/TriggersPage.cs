using _ = Atata.Tests.TriggersPage;

namespace Atata.Tests
{
    [Url("Triggers.html")]
    [VerifyTitle]
    [VerifyH1]
    public class TriggersPage : Page<_>
    {
        [InvokeMethod(nameof(OnBeforePerform), TriggerEvents.BeforeClick)]
        [InvokeMethod(nameof(OnAfterPerform), TriggerEvents.AfterClick)]
        public Button<_> Perform { get; private set; }

        public bool IsBeforePerformInvoked { get; private set; }

        public bool IsAfterPerformInvoked { get; private set; }

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
