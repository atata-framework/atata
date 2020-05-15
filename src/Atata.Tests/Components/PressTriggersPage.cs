using _ = Atata.Tests.PressTriggersPage;

namespace Atata.Tests
{
    [Url("PressTriggers")]
    public class PressTriggersPage : Page<_>
    {
        [FindById("bottom-text")]
        [PressEnd(on: TriggerEvents.BeforeClick)]
        public Text<_> BottomText { get; private set; }

        [FindById("top-text")]
        [PressHome(on: TriggerEvents.BeforeClick)]
        public Text<_> TopText { get; private set; }
    }
}
