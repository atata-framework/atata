using OpenQA.Selenium;
using _ = Atata.Tests.FrameInnerPage;

namespace Atata.Tests
{
    [VerifyH1]
    public class FrameInnerPage : Page<_>
    {
        public H1<_> Header { get; private set; }

        public TextInput<_> TextInput { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();

            IWebElement iframe = Driver.Get(By.XPath(".//iframe[@id='primary-iframe']"));
            Driver.SwitchTo().Frame(iframe);
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            Driver.SwitchTo().DefaultContent();
        }
    }
}
