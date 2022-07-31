using OpenQA.Selenium;

namespace Atata.IntegrationTests
{
    using _ = FramePage;

    [Url("frame")]
    [VerifyTitle]
    [VerifyH1]
    public class FramePage : Page<_>
    {
        private int _state;

        public H1<_> Header { get; private set; }

        [FindById("iframe-1")]
        public Frame<FrameInner1Page, _> Frame1 { get; private set; }

        [FindById("iframe-2")]
        public Frame<FrameInner2Page, _> Frame2 { get; private set; }

        [FindById("iframe-1")]
        public Frame<_> Frame1Generic { get; private set; }

        [FindById("iframe-1")]
        [GoTemporarily]
        public Frame<FrameInner1Page, _> Frame1Temporarily { get; private set; }

        [FindById("iframe-2")]
        [GoTemporarily]
        public Frame<FrameInner2Page, _> Frame2Temporarily { get; private set; }

        public ValueProvider<int, _> State =>
            CreateValueProvider("state value", () => _state);

        public _ SetState(int state)
        {
            _state = state;
            return this;
        }

        protected override void OnInit()
        {
            base.OnInit();
            _state = 0;
        }

        public FrameInner1Page SwitchToFrame1()
        {
            return SwitchToFrame<FrameInner1Page>(By.Id("iframe-1"));
        }

        public FrameInner2Page SwitchToFrame2()
        {
            return SwitchToFrame<FrameInner2Page>(By.Id("iframe-2"));
        }
    }
}
