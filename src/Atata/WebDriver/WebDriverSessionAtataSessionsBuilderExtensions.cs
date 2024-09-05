namespace Atata;

public static class WebDriverSessionAtataSessionsBuilderExtensions
{
    public static AtataContextBuilder AddWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder> configure = null) =>
        builder.Add(configure);

    public static AtataContextBuilder ConfigureWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder> configure = null) =>
        builder.Configure(configure);
}
