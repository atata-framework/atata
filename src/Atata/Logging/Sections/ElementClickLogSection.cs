namespace Atata;

public class ElementClickLogSection : LogSection
{
    public ElementClickLogSection(IWebElement element)
    {
        Message = $"Click {Stringifier.ToString(element).ToLowerFirstLetter()}";
        Level = LogLevel.Trace;
    }
}
