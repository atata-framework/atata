namespace Atata.IntegrationTests
{
    using _ = MessageBoxPage;

    [Url("messagebox")]
    [VerifyTitle]
    public class MessageBoxPage : Page<_>
    {
        public Button<_> NoneButton { get; private set; }

        [CloseAlertBox]
        public Button<_> AlertButton { get; private set; }

        [WaitForAlertBox]
        public Button<_> AlertWithDelayButton { get; private set; }

        [CloseConfirmBox]
        public Link<GoTo1Page, _> ConfirmButton { get; private set; }

        [Term("Confirm")]
        [CloseConfirmBox(false)]
        public Link<_> ConfirmButtonWithReject { get; private set; }
    }
}
