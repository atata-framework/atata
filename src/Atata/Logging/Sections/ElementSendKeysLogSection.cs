using OpenQA.Selenium;

namespace Atata
{
    public class ElementSendKeysLogSection : LogSection
    {
        public ElementSendKeysLogSection(IWebElement element, string text)
        {
            Message = $"Send keys \"{SpecialKeys.Replace(text)}\" to {Stringifier.ToString(element).ToLowerFirstLetter()}";
            Level = LogLevel.Trace;
        }
    }
}
