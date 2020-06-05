namespace Atata.Tests
{
    using _ = ScrollablePage;

    [Url("scrollable")]
    public class ScrollablePage : Page<_>
    {
        [FindById]
        [ScrollDown(TriggerEvents.BeforeHover)]
        [PressEnd(on: TriggerEvents.BeforeClick)]
        public Text<_> BottomText { get; private set; }

        [FindById]
        [ScrollUp(TriggerEvents.BeforeHover)]
        [PressHome(on: TriggerEvents.BeforeClick)]
        public Text<_> TopText { get; private set; }
    }
}
