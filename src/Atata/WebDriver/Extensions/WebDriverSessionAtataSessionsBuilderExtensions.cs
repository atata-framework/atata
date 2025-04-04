namespace Atata;

public static class WebDriverSessionAtataSessionsBuilderExtensions
{
    public static AtataContextBuilder AddWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder>? configure = null) =>
        builder.Add(configure);

    public static AtataContextBuilder ConfigureWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder> configure) =>
        builder.Configure(configure);

    public static AtataContextBuilder ConfigureWebDriver(this AtataSessionsBuilder builder, string? name, Action<WebDriverSessionBuilder> configure) =>
        builder.Configure(name, configure);

    public static AtataContextBuilder ConfigureIfExistsWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder> configure) =>
        builder.ConfigureIfExists(configure);

    public static AtataContextBuilder ConfigureIfExistsWebDriver(this AtataSessionsBuilder builder, string? name, Action<WebDriverSessionBuilder> configure) =>
        builder.ConfigureIfExists(name, configure);

    public static AtataContextBuilder ConfigureOrAddWebDriver(this AtataSessionsBuilder builder, Action<WebDriverSessionBuilder>? configure = null) =>
        builder.ConfigureOrAdd(configure);

    public static AtataContextBuilder ConfigureOrAddWebDriver(this AtataSessionsBuilder builder, string? name, Action<WebDriverSessionBuilder>? configure = null) =>
        builder.ConfigureOrAdd(name, configure);
}
