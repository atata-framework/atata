﻿namespace Atata;

public abstract class WebDriverBuilder<TBuilder, TService, TOptions>
    : WebDriverBuilder<TBuilder>, IUsesLocalBrowser
    where TBuilder : WebDriverBuilder<TBuilder, TService, TOptions>
    where TService : DriverService
    where TOptions : DriverOptions, new()
{
    private readonly string _browserName;

    private readonly List<Action<TService>> _serviceInitializers = [];
    private readonly List<Action<TOptions>> _optionsInitializers = [];

    private readonly List<int> _portsToIgnore = [];

    private Func<TService> _serviceFactory;
    private Func<TOptions> _optionsFactory;

    private string _driverPath;
    private string _driverExecutableFileName;

    private TimeSpan? _commandTimeout;

    protected WebDriverBuilder(string alias, string browserName)
        : base(alias) =>
        _browserName = browserName;

    string IUsesLocalBrowser.BrowserName => _browserName;

    protected sealed override IWebDriver CreateDriver(ILogManager logManager)
    {
        var options = _optionsFactory?.Invoke() ?? new TOptions();

        foreach (var optionsInitializer in _optionsInitializers)
            optionsInitializer(options);

        TService service = _serviceFactory?.Invoke()
            ?? CreateServiceUsingDriverParameters();

        try
        {
            foreach (var serviceInitializer in _serviceInitializers)
                serviceInitializer(service);

            CheckPortForIgnoring(service);

            logManager?.Trace($"Created {GetDriverServiceStringForLog(service)}");

            var driver = CreateDriver(service, options, _commandTimeout ?? RemoteDriverBuilder.DefaultCommandTimeout);

            if (driver is not null)
                logManager?.Trace($"Created {GetDriverStringForLog(driver)}");

            return driver;
        }
        catch
        {
            DisposeServiceSafely(service, logManager);

            throw;
        }
    }

    private static void DisposeServiceSafely(TService service, ILogManager logManager)
    {
        try
        {
            service?.Dispose();
        }
        catch (Exception exception)
        {
            logManager?.Error(exception, $"{service.GetType().Name}.Dispose() failed.");
        }
    }

    private void CheckPortForIgnoring(TService service)
    {
        if (_portsToIgnore.Contains(service.Port))
        {
            service.Port = PortUtils.FindFreePortExcept(_portsToIgnore);
        }
    }

    /// <summary>
    /// Creates the driver instance.
    /// </summary>
    /// <param name="service">The driver service.</param>
    /// <param name="options">The driver options.</param>
    /// <param name="commandTimeout">The command timeout.</param>
    /// <returns>The driver instance.</returns>
    protected abstract IWebDriver CreateDriver(TService service, TOptions options, TimeSpan commandTimeout);

    private TService CreateServiceUsingDriverParameters() =>
        _driverPath != null && _driverExecutableFileName != null
            ? CreateService(_driverPath, _driverExecutableFileName)
            : _driverPath != null
                ? CreateService(_driverPath)
                : CreateDefaultService();

    private TService CreateDefaultService() =>
        TryGetDriverPathEnvironmentVariable(out string environmentDriverPath)
            ? CreateService(environmentDriverPath)
            : CreateService();

    private bool TryGetDriverPathEnvironmentVariable(out string driverPath)
    {
        driverPath = string.IsNullOrWhiteSpace(_browserName)
            ? null
            : Environment.GetEnvironmentVariable($"{_browserName.Replace(" ", null)}Driver");

        return driverPath != null;
    }

    protected abstract TService CreateService();

    protected abstract TService CreateService(string driverPath);

    protected abstract TService CreateService(string driverPath, string driverExecutableFileName);

    /// <summary>
    /// Specifies the driver options.
    /// </summary>
    /// <param name="options">The driver options.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithOptions(TOptions options)
    {
        options.CheckNotNull(nameof(options));

        _optionsFactory = () => options;
        return (TBuilder)this;
    }

    /// <summary>
    /// Specifies the driver options factory method.
    /// </summary>
    /// <param name="optionsFactory">The factory method of the driver options.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithOptions(Func<TOptions> optionsFactory)
    {
        optionsFactory.CheckNotNull(nameof(optionsFactory));

        _optionsFactory = optionsFactory;
        return (TBuilder)this;
    }

    /// <summary>
    /// Specifies the driver options initialization method.
    /// </summary>
    /// <param name="optionsInitializer">The initialization method of the driver options.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithOptions(Action<TOptions> optionsInitializer)
    {
        optionsInitializer.CheckNotNull(nameof(optionsInitializer));

        _optionsInitializers.Add(optionsInitializer);
        return (TBuilder)this;
    }

    /// <summary>
    /// Specifies the properties map for the driver options.
    /// </summary>
    /// <param name="optionsPropertiesMap">The properties map.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithOptions(IEnumerable<KeyValuePair<string, object>> optionsPropertiesMap)
    {
        optionsPropertiesMap.CheckNotNull(nameof(optionsPropertiesMap));

        return WithOptions(opt => AtataContext.GlobalProperties.ObjectMapper.Map(optionsPropertiesMap, opt));
    }

    /// <summary>
    /// Adds the additional option to the driver options.
    /// </summary>
    /// <param name="optionName">The name of the option to add.</param>
    /// <param name="optionValue">The value of the option to add.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder AddAdditionalOption(string optionName, object optionValue)
    {
        optionName.CheckNotNullOrWhitespace(nameof(optionName));

        return WithOptions(options => options.AddAdditionalOption(optionName, optionValue));
    }

    /// <summary>
    /// Specifies the driver service factory method.
    /// </summary>
    /// <param name="serviceFactory">The factory method of the driver service.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithDriverService(Func<TService> serviceFactory)
    {
        serviceFactory.CheckNotNull(nameof(serviceFactory));

        _serviceFactory = serviceFactory;
        return (TBuilder)this;
    }

    /// <summary>
    /// Specifies the driver service initialization method.
    /// </summary>
    /// <param name="serviceInitializer">The initialization method of the driver service.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithDriverService(Action<TService> serviceInitializer)
    {
        serviceInitializer.CheckNotNull(nameof(serviceInitializer));

        _serviceInitializers.Add(serviceInitializer);
        return (TBuilder)this;
    }

    /// <summary>
    /// Specifies the properties map for the driver service.
    /// </summary>
    /// <param name="servicePropertiesMap">The properties map.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithDriverService(Dictionary<string, object> servicePropertiesMap)
    {
        servicePropertiesMap.CheckNotNull(nameof(servicePropertiesMap));

        return WithDriverService(srv => AtataContext.GlobalProperties.ObjectMapper.Map(servicePropertiesMap, srv));
    }

    /// <summary>
    /// Specifies the directory containing the driver executable file.
    /// </summary>
    /// <param name="driverPath">The directory containing the driver executable file.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithDriverPath(string driverPath)
    {
        _driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
        return (TBuilder)this;
    }

    /// <summary>
    /// Specifies that local/current directory should be used as the directory containing the driver executable file.
    /// Uses <c>AppDomain.CurrentDomain.BaseDirectory</c> as driver directory path.
    /// This configuration option makes sense for .NET Core 2.0+ project that uses driver as a project package (hosted in the same build directory).
    /// </summary>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithLocalDriverPath() =>
        WithDriverPath(AppDomain.CurrentDomain.BaseDirectory);

    /// <summary>
    /// Specifies the name of the driver executable file.
    /// </summary>
    /// <param name="driverExecutableFileName">The name of the driver executable file.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithDriverExecutableFileName(string driverExecutableFileName)
    {
        _driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
        return (TBuilder)this;
    }

    /// <summary>
    /// Specifies the host name of the service.
    /// The default value is <c>"localhost"</c>.
    /// Can be set to <c>"127.0.0.1"</c>, for example when you experience localhost resolve issues.
    /// </summary>
    /// <param name="hostName">The host name.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithHostName(string hostName) =>
        WithDriverService(x => x.HostName = hostName);

    /// <summary>
    /// Specifies the command timeout (the maximum amount of time to wait for each command).
    /// The default timeout is 60 seconds.
    /// </summary>
    /// <param name="commandTimeout">The maximum amount of time to wait for each command.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithCommandTimeout(TimeSpan commandTimeout)
    {
        _commandTimeout = commandTimeout;
        return (TBuilder)this;
    }

    /// <summary>
    /// Specifies the ports to ignore.
    /// </summary>
    /// <param name="portsToIgnore">The ports to ignore.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithPortsToIgnore(params int[] portsToIgnore) =>
        WithPortsToIgnore(portsToIgnore.AsEnumerable());

    /// <summary>
    /// Specifies the ports to ignore.
    /// </summary>
    /// <param name="portsToIgnore">The ports to ignore.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithPortsToIgnore(IEnumerable<int> portsToIgnore)
    {
        _portsToIgnore.AddRange(portsToIgnore);
        return (TBuilder)this;
    }
}
