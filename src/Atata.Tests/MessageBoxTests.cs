using NUnit.Framework;

namespace Atata.Tests
{
    public class MessageBoxTests : UITestFixture
    {
        [Test]
        public void CloseAlertBox()
        {
            Go.To<MessageBoxPage>().
                AlertButton().
                PageTitle.Should.StartWith("Message Box");
        }

        [Test]
        public void CloseConfirmBox_Accept()
        {
            Go.To<MessageBoxPage>().
                ConfirmButton().
                PageTitle.Should.StartWith("Go");
        }

        [Test]
        public void CloseConfirmBox_Reject()
        {
            Go.To<MessageBoxPage>().
                ConfirmButtonWithReject().
                PageTitle.Should.StartWith("Message Box");
        }
    }
}
