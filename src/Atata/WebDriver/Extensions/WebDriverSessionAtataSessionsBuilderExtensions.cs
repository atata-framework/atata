namespace Atata;

public static class WebDriverSessionAtataSessionsBuilderExtensions
{
    public static AtataContextBuilder AddWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder>? configure = null) =>
        builder.Add(configure);

    public static AtataContextBuilder ConfigureWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder> configure, ConfigurationMode mode = default) =>
        builder.Configure(configure, mode);

    public static AtataContextBuilder ConfigureWebDriver(this AtataSessionsBuilder builder, string? name, Action<WebDriverSessionBuilder> configure, ConfigurationMode mode = default) =>
        builder.Configure(name, configure, mode);
}
