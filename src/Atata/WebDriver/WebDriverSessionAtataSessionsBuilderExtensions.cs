namespace Atata;

public static class WebDriverSessionAtataSessionsBuilderExtensions
{
    public static WebDriverSessionBuilder AddWebDriver(this AtataSessionsBuilder builder) =>
        builder.Add<WebDriverSessionBuilder>();
}
