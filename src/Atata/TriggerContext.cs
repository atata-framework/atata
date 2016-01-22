using OpenQA.Selenium.Remote;

namespace Atata
{
    public class TriggerContext
    {
        public RemoteWebDriver Driver { get; internal set; }
        public UIComponent Component { get; internal set; }
        public ElementFinder ComponentScopeFinder { get; internal set; }
        public UIComponent ParentComponent { get; internal set; }
        public ElementFinder ParentComponentScopeFinder { get; internal set; }
    }
}
