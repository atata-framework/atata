namespace Atata;

public abstract class DriverAtataContextBuilder<TBuilder> : AtataContextBuilder, IDriverFactory
    where TBuilder : DriverAtataContextBuilder<TBuilder>
{
    protected DriverAtataContextBuilder(AtataBuildingContext buildingContext, string alias = null)
        : base(buildingContext) =>
        Alias = alias;

    /// <summary>
    /// Gets the alias.
    /// </summary>
    public string Alias { get; private set; }

    IWebDriver IDriverFactory.Create() =>
        CreateDriver();

    /// <summary>
    /// Creates the driver instance.
    /// </summary>
    /// <returns>The created <see cref="IWebDriver"/> instance.</returns>
    protected abstract IWebDriver CreateDriver();

    /// <summary>
    /// Specifies the driver alias.
    /// </summary>
    /// <param name="alias">The alias.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithAlias(string alias)
    {
        alias.CheckNotNullOrWhitespace(nameof(alias));

        Alias = alias;
        return (TBuilder)this;
    }

    protected string GetDriverServiceStringForLog(DriverService service)
    {
        StringBuilder builder = new(service.GetType().Name);

        builder.Append($" {{ Port={service.Port}");

        if (!string.IsNullOrEmpty(service.DriverServicePath) && !string.IsNullOrEmpty(service.DriverServiceExecutableName))
        {
            string executablePath = Path.Combine(service.DriverServicePath, service.DriverServiceExecutableName);
            builder.Append($", ExecutablePath={executablePath}");
        }

        builder.Append(" }");

        return builder.ToString();
    }

    protected string GetDriverStringForLog(IWebDriver driver)
    {
        StringBuilder builder = new(driver.GetType().Name);

        List<string> properties = new List<string>();

        if (!string.IsNullOrEmpty(Alias))
            properties.Add($"Alias={Alias}");

        if (driver.TryAs(out IHasSessionId driverWithSessionId))
            properties.Add($"SessionId={driverWithSessionId.SessionId}");

        if (properties.Count > 0)
            builder.Append(" { ").AppendJoined(", ", properties).Append(" }");

        return builder.ToString();
    }
}
