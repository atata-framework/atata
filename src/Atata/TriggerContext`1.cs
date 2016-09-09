using OpenQA.Selenium.Remote;

namespace Atata
{
    public class TriggerContext<TOwner>
        where TOwner : PageObject<TOwner>, IPageObject<TOwner>
    {
        public TriggerEvents Event { get; internal set; }

        public RemoteWebDriver Driver { get; internal set; }

        public ILogManager Log { get; internal set; }

        public IUIComponent<TOwner> Component { get; internal set; }
    }
}
