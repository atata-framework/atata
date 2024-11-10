namespace Atata.WebDriver;

#warning Temporarily copied driver setup methods to this class.
public static class WebDriverAtataContextBuilderExtensions
{
    /// <summary>
    /// <para>
    /// Sets up drivers with auto version detection for the local browsers to use.
    /// Gets the name of the local browsers to use from <see cref="WebDriverSessionBuilder.LocalBrowserToUseName"/> property
    /// of <see cref="WebDriverSessionBuilder"/> session builders.
    /// Then invokes <c>Atata.WebDriverSetup.DriverSetup.AutoSetUpSafely(...)</c> static method
    /// from <c>Atata.WebDriverSetup</c> package.
    /// </para>
    /// <para>
    /// In order to use this method,
    /// ensure that <c>Atata.WebDriverSetup</c> package is installed.
    /// </para>
    /// </summary>
    /// <param name="builder">The builder.</param>
    public static void AutoSetUpDriversToUse(this AtataContextBuilder builder)
    {
        var browserNames = builder.Sessions.Builders.OfType<WebDriverSessionBuilder>()
            .Where(x => x.UsesLocalBrowser)
            .Select(x => x.LocalBrowserToUseName)
            .Distinct()
            .ToArray();

        if (browserNames.Length > 0)
            InvokeAutoSetUpSafelyMethodOfDriverSetup(browserNames);
    }

    /// <inheritdoc cref="AutoSetUpDriversToUse"/>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task AutoSetUpDriversToUseAsync(this AtataContextBuilder builder) =>
        await Task.Run(() => AutoSetUpDriversToUse(builder))
            .ConfigureAwait(false);

    /// <summary>
    /// <para>
    /// Sets up drivers with auto version detection for the local configured browsers.
    /// Gets the names of configured local browsers from <see cref="WebDriverSessionBuilder.LocalBrowserToUseName"/> property
    /// of <see cref="WebDriverSessionBuilder"/> session builders.
    /// Then invokes <c>Atata.WebDriverSetup.DriverSetup.AutoSetUpSafely(...)</c> static method
    /// from <c>Atata.WebDriverSetup</c> package.
    /// </para>
    /// <para>
    /// In order to use this method,
    /// ensure that <c>Atata.WebDriverSetup</c> package is installed.
    /// </para>
    /// </summary>
    /// <param name="builder">The builder.</param>
    public static void AutoSetUpConfiguredDrivers(this AtataContextBuilder builder)
    {
        var browserNames = builder.Sessions.Builders.OfType<WebDriverSessionBuilder>()
            .SelectMany(x => x.ConfiguredLocalBrowserNames)
            .Distinct()
            .ToArray();

        InvokeAutoSetUpSafelyMethodOfDriverSetup(browserNames);
    }

    /// <inheritdoc cref="AutoSetUpConfiguredDrivers"/>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task AutoSetUpConfiguredDriversAsync(this AtataContextBuilder builder) =>
        await Task.Run(() => AutoSetUpConfiguredDrivers(builder))
            .ConfigureAwait(false);

    private static void InvokeAutoSetUpSafelyMethodOfDriverSetup(IEnumerable<string> browserNames)
    {
        Type driverSetupType = Type.GetType("Atata.WebDriverSetup.DriverSetup,Atata.WebDriverSetup", true);

        var setUpMethod = driverSetupType.GetMethodWithThrowOnError(
            "AutoSetUpSafely",
            BindingFlags.Public | BindingFlags.Static);

        setUpMethod.InvokeStaticAsLambda(browserNames);
    }
}
