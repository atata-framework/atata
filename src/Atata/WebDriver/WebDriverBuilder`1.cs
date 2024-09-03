namespace Atata;

public abstract class WebDriverBuilder<TBuilder> : IWebDriverFactory
    where TBuilder : WebDriverBuilder<TBuilder>
{
    private Func<IWebDriver, bool> _initialHealthCheckFunction = CheckHealthByRequestingDriverUrl;

    protected WebDriverBuilder(string alias = null) =>
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

    /// <summary>
    /// Gets a value indicating whether to execute an initial health check.
    /// The default value is <see langword="false"/>.
    /// </summary>
    public bool InitialHealthCheck { get; private set; }

    IWebDriver IWebDriverFactory.Create(ILogManager logManager)
    {
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
                logManager?.Warn(exception, $"{creationErrorMessage} Will retry.");
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
                    logManager?.Warn(exception, healthCheckWarningMessage);
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

                    logManager?.Warn(healthCheckWarningMessage);
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
    /// <param name="logManager">The log manager, which can be <see langword="null"/>.</param>
    /// <returns>The created <see cref="IWebDriver"/> instance.</returns>
    protected abstract IWebDriver CreateDriver(ILogManager logManager);

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

    /// <summary>
    /// Enables or disables an initial health check.
    /// By default it is disabled.
    /// When enabled, the default health check function requests <see cref="IWebDriver.Url"/>.
    /// The health check function can be changed by using <see cref="WithInitialHealthCheckFunction(Func{IWebDriver, bool})"/> method.
    /// </summary>
    /// <param name="enable">Whether to enable an initial health check.</param>
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
        function.CheckNotNull(nameof(function));

        _initialHealthCheckFunction = function;
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

    private static bool CheckHealthByRequestingDriverUrl(IWebDriver driver)
    {
        _ = driver.Url;
        return true;
    }
}
