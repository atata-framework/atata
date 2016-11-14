using _ = Atata.Tests.MessageBoxPage;

namespace Atata.Tests
{
    [Url("MessageBox.html")]
    [VerifyTitle]
    public class MessageBoxPage : Page<_>
    {
        [CloseAlertBox]
        public ButtonDelegate<_> AlertButton { get; private set; }

        [CloseConfirmBox]
        public LinkDelegate<GoTo1Page, _> ConfirmButton { get; private set; }

        [Term("Confirm")]
        [CloseConfirmBox(false)]
        public LinkDelegate<_> ConfirmButtonWithReject { get; private set; }
    }
}
