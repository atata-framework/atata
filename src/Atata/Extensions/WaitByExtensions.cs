#nullable enable

namespace Atata;

public static class WaitByExtensions
{
    public static By GetBy(this WaitBy waitBy, string selector) =>
        waitBy switch
        {
            WaitBy.Id => By.Id(selector),
            WaitBy.Name => By.Name(selector),
            WaitBy.Class => By.ClassName(selector),
            WaitBy.Css => By.CssSelector(selector),
            WaitBy.XPath => By.XPath(selector),
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(waitBy, nameof(waitBy))
        };
}
