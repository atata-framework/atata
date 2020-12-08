using OpenQA.Selenium;

namespace Atata
{
    public class ElementClearLogSection : LogSection
    {
        public ElementClearLogSection(IWebElement element)
        {
            Message = $"Clear {Stringifier.ToString(element).ToLowerFirstLetter()}";
            Level = LogLevel.Trace;
        }
    }
}
