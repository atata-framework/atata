using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Atata;

public class RemoteDriverBuilder : WebDriverBuilder<RemoteDriverBuilder>
{
    /// <summary>
    /// The default command timeout is <c>60</c> seconds.
    /// </summary>
    public static readonly TimeSpan DefaultCommandTimeout = TimeSpan.FromSeconds(60);

    private List<Action<DriverOptions>> _optionsInitializers = [];

    private Uri _remoteAddress = new("http://127.0.0.1:4444/");

    private Func<DriverOptions>? _optionsFactory;

    private Func<ICapabilities>? _capabilitiesFactory;

    private TimeSpan? _commandTimeout;

    public RemoteDriverBuilder()
        : base(WebDriverAliases.Remote)
    {
    }

    protected sealed override IWebDriver CreateDriver(ILogManager logManager)
    {
        ICapabilities capabilities = CreateCapabilities();

        var driver = CreateDriver(
            _remoteAddress,
            capabilities,
            _commandTimeout ?? DefaultCommandTimeout);

        if (driver is not null)
            logManager?.Trace($"Created {GetDriverStringForLog(driver)}");

        return driver!;
    }

    protected virtual IWebDriver CreateDriver(
        Uri remoteAddress,
        ICapabilities capabilities,
        TimeSpan commandTimeout) =>
        new RemoteWebDriver(remoteAddress, capabilities, commandTimeout);

    protected virtual ICapabilities CreateCapabilities()
    {
        var options = _optionsFactory?.Invoke();

        if (options != null)
        {
            foreach (var optionsInitializer in _optionsInitializers)
                optionsInitializer(options);

            return options.ToCapabilities();
        }
        else
        {
            return _capabilitiesFactory?.Invoke()
                ?? throw new InvalidOperationException(
                    $"Type or instance of {nameof(DriverOptions)} is not set. " +
                    $"Use one of {nameof(RemoteDriverBuilder)}.{nameof(WithOptions)} methods to set driver options type or instance.");
        }
    }

    /// <summary>
    /// Specifies the remote address URI.
    /// The default value is <c>"http://127.0.0.1:4444/"</c>.
    /// </summary>
    /// <param name="remoteAddress">URI containing the address of the WebDriver remote server (e.g. http://127.0.0.1:4444/wd/hub).</param>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder WithRemoteAddress(Uri remoteAddress)
    {
        _remoteAddress = remoteAddress;
        return this;
    }

    /// <summary>
    /// Specifies the remote address URI.
    /// The default value is <c>"http://127.0.0.1:4444/"</c>.
    /// </summary>
    /// <param name="remoteAddress">URI string containing the address of the WebDriver remote server (e.g. http://127.0.0.1:4444/wd/hub).</param>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder WithRemoteAddress(string remoteAddress)
    {
        remoteAddress.CheckNotNullOrWhitespace(nameof(remoteAddress));

        _remoteAddress = new Uri(remoteAddress);
        return this;
    }

    /// <summary>
    /// Specifies the type of the driver options.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder WithOptions<TOptions>()
        where TOptions : DriverOptions, new()
    {
        _optionsFactory = () => new TOptions();
        return this;
    }

    /// <summary>
    /// Specifies the driver options.
    /// </summary>
    /// <param name="options">The driver options.</param>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder WithOptions(DriverOptions options)
    {
        options.CheckNotNull(nameof(options));

        _optionsFactory = () => options;
        return this;
    }

    /// <summary>
    /// Specifies the driver options factory method.
    /// </summary>
    /// <param name="optionsFactory">The factory method of the driver options.</param>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder WithOptions(Func<DriverOptions> optionsFactory)
    {
        optionsFactory.CheckNotNull(nameof(optionsFactory));

        _optionsFactory = optionsFactory;
        return this;
    }

    /// <summary>
    /// Specifies the driver options initialization method.
    /// </summary>
    /// <param name="optionsInitializer">The initialization method of the driver options.</param>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder WithOptions(Action<DriverOptions> optionsInitializer)
    {
        optionsInitializer.CheckNotNull(nameof(optionsInitializer));

        _optionsInitializers.Add(optionsInitializer);
        return this;
    }

    /// <summary>
    /// Specifies the capabilities.
    /// </summary>
    /// <param name="capabilities">The driver capabilities.</param>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder WithCapabilities(ICapabilities capabilities)
    {
        capabilities.CheckNotNull(nameof(capabilities));

        _capabilitiesFactory = () => capabilities;
        return this;
    }

    /// <summary>
    /// Specifies the capabilities factory method.
    /// </summary>
    /// <param name="capabilitiesFactory">The factory method of the driver capabilities.</param>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder WithCapabilities(Func<ICapabilities> capabilitiesFactory)
    {
        capabilitiesFactory.CheckNotNull(nameof(capabilitiesFactory));

        _capabilitiesFactory = capabilitiesFactory;
        return this;
    }

    /// <summary>
    /// Adds the additional option to the driver options.
    /// </summary>
    /// <param name="optionName">The name of the option to add.</param>
    /// <param name="optionValue">The value of the option to add.</param>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder AddAdditionalOption(string optionName, object optionValue)
    {
        optionName.CheckNotNullOrWhitespace(nameof(optionName));

        return WithOptions(options => options.AddAdditionalOption(optionName, optionValue));
    }

    /// <summary>
    /// Adds the additional browser option to the driver options.
    /// </summary>
    /// <param name="optionName">The name of the option to add.</param>
    /// <param name="optionValue">The value of the option to add.</param>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
    {
        optionName.CheckNotNullOrWhitespace(nameof(optionName));

        return WithOptions(options =>
        {
            if (options is ChromeOptions chromeOptions)
                chromeOptions.AddAdditionalChromeOption(optionName, optionValue);
            else if (options is EdgeOptions edgeOptions)
                edgeOptions.AddAdditionalEdgeOption(optionName, optionValue);
            else if (options is FirefoxOptions firefoxOptions)
                firefoxOptions.AddAdditionalFirefoxOption(optionName, optionValue);
            else if (options is InternetExplorerOptions internetExplorerOptions)
                internetExplorerOptions.AddAdditionalInternetExplorerOption(optionName, optionValue);
            else
                throw new InvalidOperationException(
                    $"Cannot add \"{optionName}\"={optionValue} additional browser option to options " +
                    $"of {options.GetType()} type as it doesn't have an appropriate method.");
        });
    }

    /// <summary>
    /// Specifies the command timeout (the maximum amount of time to wait for each command).
    /// The default value is <c>60</c> seconds.
    /// </summary>
    /// <param name="commandTimeout">The maximum amount of time to wait for each command.</param>
    /// <returns>The same builder instance.</returns>
    public RemoteDriverBuilder WithCommandTimeout(TimeSpan commandTimeout)
    {
        _commandTimeout = commandTimeout;
        return this;
    }

    protected override void OnClone(RemoteDriverBuilder copy)
    {
        base.OnClone(copy);

        copy._optionsInitializers = [.. _optionsInitializers];
    }
}
