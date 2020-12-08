using System.Text;
using OpenQA.Selenium;

namespace Atata
{
    public class ElementFindLogSection : LogSection
    {
        public ElementFindLogSection(ISearchContext searchContext, By by, bool multiple = false)
        {
            StringBuilder builder = new StringBuilder("Find ");

            Visibility visibility = by.GetSearchOptionsOrDefault().Visibility;

            if (visibility != Visibility.Any)
                builder.Append($"{visibility.ToString(TermCase.Lower)} ");

            builder.Append("element");

            if (multiple)
                builder.Append('s');

            builder.Append($" by {by.ToDescriptiveString()} in {ResolveSearchContextName(searchContext)}");

            Message = builder.ToString();
            Level = LogLevel.Trace;
        }

        private static string ResolveSearchContextName(ISearchContext searchContext)
        {
            return searchContext is IWebDriver
                ? searchContext.GetType().Name
                : Stringifier.ToString(searchContext);
        }
    }
}
