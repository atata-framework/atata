using OpenQA.Selenium;

namespace Atata
{
    [PageObjectDefinition(ComponentTypeName = "page", IgnoreNameEndings = "Page,PageObject")]
    public class Page<T> : PageObject<T>
        where T : Page<T>
    {
        protected override By CreateScopeBy()
        {
            return By.TagName("body");
        }
    }
}
