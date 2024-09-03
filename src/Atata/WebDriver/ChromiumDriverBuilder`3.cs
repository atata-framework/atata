using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using OpenQA.Selenium.Chromium;

namespace Atata;

/// <summary>
/// Represents the base class for Chromium-based web driver builder.
/// </summary>
/// <typeparam name="TBuilder">The type of the builder.</typeparam>
/// <typeparam name="TService">The type of the driver service.</typeparam>
/// <typeparam name="TOptions">The type of the options.</typeparam>
public abstract class ChromiumDriverBuilder<TBuilder, TService, TOptions>
    : WebDriverBuilder<TBuilder, TService, TOptions>
    where TBuilder : ChromiumDriverBuilder<TBuilder, TService, TOptions>
    where TService : ChromiumDriverService
    where TOptions : ChromiumOptions, new()
{
    protected ChromiumDriverBuilder(string alias, string browserName)
        : base(alias, browserName)
    {
    }

    /// <summary>
    /// Adds arguments to be appended to the browser executable command line.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithArguments(params string[] arguments) =>
        WithArguments(arguments.AsEnumerable());

    /// <summary>
    /// Adds arguments to be appended to the browser executable command line.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithArguments(IEnumerable<string> arguments) =>
        WithOptions(options => options.AddArguments(arguments));

    /// <summary>
    /// Adds the <c>download.default_directory</c> user profile preference to options
    /// with the value of Artifacts directory path.
    /// </summary>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithArtifactsAsDownloadDirectory() =>
        WithDownloadDirectory(() => AtataContext.Current.ArtifactsPath);

    /// <summary>
    /// Adds the <c>download.default_directory</c> user profile preference to options
    /// with the value specified by <paramref name="directoryPath"/>.
    /// </summary>
    /// <param name="directoryPath">The directory path.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithDownloadDirectory(string directoryPath)
    {
        directoryPath.CheckNotNull(nameof(directoryPath));

        return WithDownloadDirectory(() => directoryPath);
    }

    /// <summary>
    /// Adds the <c>download.default_directory</c> user profile preference to options
    /// with the value specified by <paramref name="directoryPathBuilder"/>.
    /// </summary>
    /// <param name="directoryPathBuilder">The directory path builder.</param>
    /// <returns>The same builder instance.</returns>
    public TBuilder WithDownloadDirectory(Func<string> directoryPathBuilder)
    {
        directoryPathBuilder.CheckNotNull(nameof(directoryPathBuilder));

        return WithOptions(x => x
            .AddUserProfilePreference("download.default_directory", directoryPathBuilder.Invoke()));
    }

    protected void ReplaceLocalhostInDebuggerAddress(ICapabilities capabilities, string optionsCapabilityName)
    {
        if (capabilities.GetCapability(optionsCapabilityName) is Dictionary<string, object> chromiumOptions
            && chromiumOptions.TryGetValue("debuggerAddress", out object debuggerAddressObject)
            && debuggerAddressObject is string debuggerAddress
            && debuggerAddress.Contains("localhost:"))
        {
            string portString = debuggerAddress.Substring(debuggerAddress.IndexOf(':') + 1);

            if (int.TryParse(portString, out int port))
            {
                IPEndPoint ipEndPoint = GetIPEndPoint(port);

                if (ipEndPoint?.AddressFamily == AddressFamily.InterNetwork)
                    chromiumOptions["debuggerAddress"] = $"127.0.0.1:{port}";
            }
        }
    }

    private static IPEndPoint GetIPEndPoint(int port)
    {
        try
        {
            return IPGlobalProperties.GetIPGlobalProperties()
                .GetActiveTcpListeners()
                .FirstOrDefault(x => x.Port == port);
        }
        catch
        {
            return null;
        }
    }
}
