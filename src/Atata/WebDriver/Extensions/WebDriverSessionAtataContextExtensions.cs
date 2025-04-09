namespace Atata;

public static class WebDriverSessionAtataContextExtensions
{
    public static IWebDriver GetWebDriver(this AtataContext atataContext) =>
        atataContext.Sessions.Get<WebDriverSession>().Driver;
}
