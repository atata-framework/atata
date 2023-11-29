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

    /// <summary>
    /// Gets the count of possible driver creation retries in case exceptions occur during creation.
    /// The default value is <c>2</c>.
    /// </summary>
    public int CreateRetries { get; private set; } = 2;

    IWebDriver IDriverFactory.Create()
    {
        int retriesLeft = CreateRetries;

        while (true)
        {
            try
            {
                return CreateDriver();
            }
            catch (Exception exception)
            {
                if (retriesLeft == 0)
                    throw;

                // TODO: v3. Add ILogManager parameter to IDriverFactory.Create method and use it below.
                AtataContext.Current?.Log.Warn(exception, "Failed to create driver. Will retry.");
                retriesLeft--;
            }
        }
    }

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

    /// <summary>
    /// Specifies the count of possible driver creation retries in case exceptions occur during creation.
    /// The default value is <c>2</c>.
    /// Set <c>0</c> to omit retries.
    /// </summary>
    /// <param name="createRetries">The count of retries.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithCreateRetries(int createRetries)
    {
        createRetries.CheckGreaterOrEqual(nameof(createRetries), 0);

        CreateRetries = createRetries;
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

        List<string> properties = [];

        if (!string.IsNullOrEmpty(Alias))
            properties.Add($"Alias={Alias}");

        if (driver.TryAs(out IHasSessionId driverWithSessionId))
            properties.Add($"SessionId={driverWithSessionId.SessionId}");

        if (properties.Count > 0)
            builder.Append(" { ").AppendJoined(", ", properties).Append(" }");

        return builder.ToString();
    }
}
