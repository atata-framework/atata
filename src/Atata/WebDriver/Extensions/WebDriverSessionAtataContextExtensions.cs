#nullable enable

namespace Atata;

public static class WebDriverSessionAtataContextExtensions
{
    public static WebDriverSession GetWebDriverSession(this AtataContext atataContext) =>
        atataContext.Sessions.Get<WebDriverSession>();

    public static IWebDriver GetWebDriver(this AtataContext atataContext) =>
        atataContext.GetWebDriverSession().Driver;
}
