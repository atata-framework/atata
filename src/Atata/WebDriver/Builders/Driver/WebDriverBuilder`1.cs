namespace Atata;

/// <summary>
/// Represents a base class for building web driver instances.
/// </summary>
/// <typeparam name="TBuilder">The type of the builder.</typeparam>
public abstract class WebDriverBuilder<TBuilder> : IWebDriverFactory, ICloneable
    where TBuilder : WebDriverBuilder<TBuilder>
{
    private Func<IWebDriver, bool> _initialHealthCheckFunction = CheckHealthByRequestingDriverUrl;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebDriverBuilder{TBuilder}"/> class.
    /// </summary>
    /// <param name="alias">The alias for the driver.</param>
    protected WebDriverBuilder(string? alias = null) =>
        Alias = alias;

    /// <summary>
    /// Gets the alias of the driver.
    /// </summary>
    public string? Alias { get; private set; }

    /// <summary>
    /// Gets the count of possible driver creation retries in case exceptions occur during creation.
    /// The default value is <c>2</c>.
    /// </summary>
    public int CreateRetries { get; private set; } = 2;

    /// <summary>
    /// Gets a value indicating whether to execute an initial health check.
    /// The default value is <see langword="false"/>.
    /// </summary>
    public bool InitialHealthCheck { get; private set; }

    IWebDriver IWebDriverFactory.Create(ILogManager logManager)
    {
        Guard.ThrowIfNull(logManager);

        int retriesLeft = CreateRetries;

        while (true)
        {
            IWebDriver driver;
            const string creationErrorMessage = "Failed to create driver.";

            try
            {
                driver = CreateDriver(logManager);
            }
            catch (Exception exception) when (retriesLeft > 0)
            {
                logManager.Warn(exception, $"{creationErrorMessage} Will retry.");
                retriesLeft--;
                continue;
            }
            catch (Exception exception)
            {
                throw new WebDriverInitializationException(creationErrorMessage, exception);
            }

            const string healthCheckErrorMessage = "Driver initial health check failed.";
            const string healthCheckWarningMessage = $"{healthCheckErrorMessage} Will retry with new driver.";

            if (InitialHealthCheck && _initialHealthCheckFunction is not null)
            {
                bool isHealthOk;

                try
                {
                    isHealthOk = _initialHealthCheckFunction.Invoke(driver);
                }
                catch (Exception exception) when (retriesLeft > 0)
                {
                    logManager.Warn(exception, healthCheckWarningMessage);
                    DisposeSafely(driver);
                    retriesLeft--;
                    continue;
                }
                catch (Exception exception)
                {
                    throw new WebDriverInitializationException(healthCheckErrorMessage, exception);
                }

                if (!isHealthOk)
                {
                    if (retriesLeft == 0)
                        throw new WebDriverInitializationException(healthCheckErrorMessage);

                    logManager.Warn(healthCheckWarningMessage);
                    DisposeSafely(driver);
                    retriesLeft--;
                    continue;
                }
            }

            return driver;
        }
    }

    private static void DisposeSafely(IDisposable disposable)
    {
        try
        {
            disposable.Dispose();
        }
        catch
        {
            // Do nothing.
        }
    }

    /// <summary>
    /// Creates the driver instance.
    /// </summary>
    /// <param name="logManager">The log manager.</param>
    /// <returns>The created <see cref="IWebDriver"/> instance.</returns>
    protected abstract IWebDriver CreateDriver(ILogManager logManager);

    /// <summary>
    /// Specifies the driver alias.
    /// </summary>
    /// <param name="alias">The alias.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithAlias(string alias)
    {
        Guard.ThrowIfNullOrWhitespace(alias);

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
        Guard.ThrowIfLessThan(createRetries, 0);

        CreateRetries = createRetries;
        return (TBuilder)this;
    }

    /// <summary>
    /// Enables or disables an initial health check.
    /// By default it is disabled.
    /// When enabled, the default health check function requests <see cref="IWebDriver.Url"/>.
    /// The health check function can be changed by using <see cref="WithInitialHealthCheckFunction(Func{IWebDriver, bool})"/> method.
    /// </summary>
    /// <param name="enable">
    /// Whether to enable an initial health check.
    /// The default value is <see langword="true"/>.
    /// </param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithInitialHealthCheck(bool enable = true)
    {
        InitialHealthCheck = enable;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the initial health check function.
    /// The default function requests <see cref="IWebDriver.Url"/>.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithInitialHealthCheckFunction(Func<IWebDriver, bool> function)
    {
        Guard.ThrowIfNull(function);

        _initialHealthCheckFunction = function;
        return (TBuilder)this;
    }

    /// <summary>
    /// Gets a string representation of the driver service for logging purposes.
    /// </summary>
    /// <param name="service">The driver service.</param>
    /// <returns>A string representation of the driver service.</returns>
    protected string GetDriverServiceStringForLog(DriverService service)
    {
        StringBuilder builder = new(service.GetType().ToStringInShortForm());

        builder.Append($" {{ Port={service.Port}");

        if (service.DriverServicePath?.Length > 0 && service.DriverServiceExecutableName?.Length > 0)
        {
            string executablePath = Path.Combine(service.DriverServicePath, service.DriverServiceExecutableName);
            builder.Append($", ExecutablePath={executablePath}");
        }

        builder.Append(" }");

        return builder.ToString();
    }

    /// <summary>
    /// Gets a string representation of the driver for logging purposes.
    /// </summary>
    /// <param name="driver">The web driver.</param>
    /// <returns>A string representation of the driver.</returns>
    protected string GetDriverStringForLog(IWebDriver driver)
    {
        StringBuilder builder = new(driver.GetType().ToStringInShortForm());

        List<string> properties = [];

        if (Alias?.Length > 0)
            properties.Add($"Alias={Alias}");

        if (driver.TryAs(out IHasSessionId? driverWithSessionId))
            properties.Add($"SessionId={driverWithSessionId.SessionId}");

        if (properties.Count > 0)
            builder.Append(" { ").AppendJoined(", ", properties).Append(" }");

        return builder.ToString();
    }

    private static bool CheckHealthByRequestingDriverUrl(IWebDriver driver)
    {
        _ = driver.Url;
        return true;
    }

    object ICloneable.Clone()
    {
        var copy = (TBuilder)MemberwiseClone();

        OnClone(copy);

        return copy;
    }

    /// <summary>
    /// Called when the builder is cloned.
    /// </summary>
    /// <param name="copy">The cloned builder instance.</param>
    protected virtual void OnClone(TBuilder copy)
    {
    }
}
