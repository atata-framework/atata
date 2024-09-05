namespace Atata;

public static class WebDriverSessionAtataSessionsBuilderExtensions
{
    public static WebDriverSessionBuilder AddWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder> configure = null)
    {
        var sessionBuilder = builder.Add<WebDriverSessionBuilder>();
        configure?.Invoke(sessionBuilder);
        return sessionBuilder;
    }
}
