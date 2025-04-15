namespace Atata;

public static class WebDriverSessionAtataSessionCollectionExtensions
{
    /// <summary>
    /// Creates a new <see cref="WebDriverSessionBuilder"/> and adds it to the collection.
    /// </summary>
    /// <param name="collection">The session collection.</param>
    /// <param name="configure">An action delegate to configure the <see cref="WebDriverSessionBuilder"/>.</param>
    /// <returns>The created <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public static WebDriverSessionBuilder AddWebDriver(this AtataSessionCollection collection, Action<WebDriverSessionBuilder>? configure = null) =>
        collection.Add(configure);
}
