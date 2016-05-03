using OpenQA.Selenium.Remote;

namespace Atata
{
    public class TriggerContext<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public TriggerEvents Event { get; internal set; }

        public RemoteWebDriver Driver { get; internal set; }
        public ILogManager Log { get; internal set; }

        public UIComponent<TOwner> Component { get; internal set; }
        public IScopeLocator ComponentScopeLocator { get; internal set; }
        public UIComponent<TOwner> ParentComponent { get; internal set; }
        public IScopeLocator ParentComponentScopeLocator { get; internal set; }
    }
}
