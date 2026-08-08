namespace Atata;

public static class AtataContextBuilderExtensions
{
    /// <summary>
    /// <para>
    /// Adds <see cref="SetUpWebDriversEventHandler"/> instance to the <see cref="AtataContextBuilder.EventSubscriptions"/> collection.
    /// </para>
    /// <para>
    /// The <see cref="SetUpWebDriversEventHandler"/> sets up drivers with auto version detection for the specified browsers.
    /// </para>
    /// <para>
    /// In order to use this method,
    /// ensure that <c>Atata.WebDriverSetup</c> package is installed.
    /// </para>
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="browserNames">The browser names.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder SetUpWebDrivers(this AtataContextBuilder builder, params string[] browserNames)
    {
        builder.EventSubscriptions.Add(new SetUpWebDriversEventHandler(browserNames));
        return builder;
    }

    /// <summary>
    /// <para>
    /// Adds <see cref="SetUpWebDriversForUseEventHandler"/> instance to the <see cref="AtataContextBuilder.EventSubscriptions"/> collection.
    /// </para>
    /// <para>
    /// The <see cref="SetUpWebDriversForUseEventHandler"/> sets up drivers with automatic version detection for the local browsers,
    /// which are specified in the preconfigured <see cref="WebDriverSessionBuilder"/> instances as drivers to use.
    /// </para>
    /// <para>
    /// In order to use this method,
    /// ensure that <c>Atata.WebDriverSetup</c> package is installed.
    /// </para>
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder SetUpWebDriversForUse(this AtataContextBuilder builder)
    {
        builder.EventSubscriptions.Add(SetUpWebDriversForUseEventHandler.Instance);
        return builder;
    }

    /// <summary>
    /// <para>
    /// Adds <see cref="SetUpWebDriversConfiguredEventHandler"/> instance to the <see cref="AtataContextBuilder.EventSubscriptions"/> collection.
    /// </para>
    /// <para>
    /// The <see cref="SetUpWebDriversConfiguredEventHandler"/> sets up drivers with automatic version detection for the local browsers,
    /// which are specified in the preconfigured <see cref="WebDriverSessionBuilder"/> instances as configured drivers.
    /// </para>
    /// <para>
    /// In order to use this method,
    /// ensure that <c>Atata.WebDriverSetup</c> package is installed.
    /// </para>
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder SetUpWebDriversConfigured(this AtataContextBuilder builder)
    {
        builder.EventSubscriptions.Add(SetUpWebDriversConfiguredEventHandler.Instance);
        return builder;
    }
}
