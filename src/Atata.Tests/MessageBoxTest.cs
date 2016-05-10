using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class MessageBoxTest : TestBase
    {
        [Test]
        public void CloseAlertBox()
        {
            Go.To<MessageBoxPage>().
                AlertButton().
                PageTitle.VerifyStartsWith("Message Box");
        }

        [Test]
        public void CloseConfirmBox_Accept()
        {
            Go.To<MessageBoxPage>().
                ConfirmButton().
                PageTitle.VerifyStartsWith("Go");
        }

        [Test]
        public void CloseConfirmBox_Reject()
        {
            Go.To<MessageBoxPage>().
                ConfirmButtonWithReject().
                PageTitle.VerifyStartsWith("Message Box");
        }
    }
}
