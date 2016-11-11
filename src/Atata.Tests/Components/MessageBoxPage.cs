using _ = Atata.Tests.MessageBoxPage;

namespace Atata.Tests
{
    [Url("MessageBox.html")]
    [VerifyTitle]
    public class MessageBoxPage : Page<_>
    {
        [CloseAlertBox]
        public Button<_> AlertButton { get; private set; }

        [CloseConfirmBox]
        public Link<GoTo1Page, _> ConfirmButton { get; private set; }

        [Term("Confirm")]
        [CloseConfirmBox(false)]
        public Link<_> ConfirmButtonWithReject { get; private set; }
    }
}
