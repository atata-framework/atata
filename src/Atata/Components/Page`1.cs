using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the whole HTML page. Uses the body tag as a scope.
    /// </summary>
    /// <typeparam name="T">The type of the owner page object.</typeparam>
    /// <seealso cref="PageObject{T}" />
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
