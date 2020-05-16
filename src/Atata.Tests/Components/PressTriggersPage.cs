namespace Atata.Tests
{
    using _ = PressTriggersPage;

    [Url("presstriggers")]
    public class PressTriggersPage : Page<_>
    {
        [FindById]
        [PressEnd(on: TriggerEvents.BeforeClick)]
        public Text<_> BottomText { get; private set; }

        [FindById]
        [PressHome(on: TriggerEvents.BeforeClick)]
        public Text<_> TopText { get; private set; }
    }
}
