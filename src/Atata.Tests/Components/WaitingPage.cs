using _ = Atata.Tests.WaitingPage;

namespace Atata.Tests
{
    [NavigateTo("Waiting.html")]
    [VerifyTitle]
    [VerifyH1]
    public class WaitingPage : Page<_>
    {
        [Term("Wait")]
        public Button<_> ButtonWithoutWait { get; private set; }

        [Term("Wait")]
        [WaitForElementByClass("processing-block")]
        public Button<_> ButtonWithMissingOrHiddenWait { get; private set; }

        [Term("Wait")]
        [WaitForElementByCss(".processing-block", WaitUntil.VisibleAndHidden)]
        public Button<_> ButtonWithVisibleAndHiddenWait { get; private set; }

        [Term("Wait")]
        [WaitForElementByCss(".nonexistent-block", WaitUntil.VisibleAndMissing, PresenceTimeout = 2, ThrowOnPresenceFailure = false)]
        public Button<_> ButtonWithVisibleAndMissingWait { get; private set; }

        [Term("Wait")]
        [WaitForElementByCss(".nonexistent-block", WaitUntil.VisibleAndMissing, PresenceTimeout = 1)]
        public Button<_> ButtonWithVisibleAndMissingNonExistentWait { get; private set; }

        [Term("Wait")]
        [WaitForElementByCss(".result-block", WaitUntil.HiddenAndVisible)]
        public Button<_> ButtonWithHiddenAndVisibleWait { get; private set; }

        [FindByClass("result-block")]
        public Text<_> Result { get; private set; }
    }
}
