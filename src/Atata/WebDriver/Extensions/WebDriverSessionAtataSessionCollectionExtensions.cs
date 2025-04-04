namespace Atata;

public static class WebDriverSessionAtataSessionCollectionExtensions
{
    public static WebDriverSessionBuilder AddWebDriver(this AtataSessionCollection collection, Action<WebDriverSessionBuilder>? configure = null) =>
        collection.Add(configure);
}
