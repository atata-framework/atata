using _ = Atata.Tests.WaitingPage;

namespace Atata.Tests
{
    [Url("Waiting.html")]
    [VerifyTitle]
    [VerifyH1]
    public class WaitingPage : Page<_>
    {
        [Term("Wait")]
        public ButtonDelegate<_> ButtonWithoutWait { get; private set; }

        [Term("Wait")]
        [WaitForElement(WaitBy.Class, "processing-block")]
        public ButtonDelegate<_> ButtonWithMissingOrHiddenWait { get; private set; }

        [Term("Wait")]
        [WaitForElement(WaitBy.Class, "processing-block", WaitUntil.VisibleThenHidden)]
        public ButtonDelegate<_> ButtonWithVisibleAndHiddenWait { get; private set; }

        [Term("Wait")]
        [WaitForElement(WaitBy.Css, ".nonexistent-block", WaitUntil.VisibleThenMissing, PresenceTimeout = 2, ThrowOnPresenceFailure = false)]
        public ButtonDelegate<_> ButtonWithVisibleAndMissingWait { get; private set; }

        [Term("Wait")]
        [WaitForElement(WaitBy.Css, ".nonexistent-block", WaitUntil.VisibleThenMissing, PresenceTimeout = 1)]
        public ButtonDelegate<_> ButtonWithVisibleAndMissingNonExistentWait { get; private set; }

        [Term("Wait")]
        [WaitForElement(WaitBy.Class, "result-block", WaitUntil.HiddenThenVisible)]
        public ButtonDelegate<_> ButtonWithHiddenAndVisibleWait { get; private set; }

        [FindByClass("result-block")]
        public Text<_> Result { get; private set; }

        [Term("Go to WaitingOnInit page")]
        public Link<WaitingOnInitPage, _> GoToWaitingOnInitPage { get; private set; }

        [Term("Wait and go to WaitingOnInit page")]
        public Button<WaitingOnInitPage, _> WaitAndGoToWaitingOnInitPage { get; private set; }
    }
}
