namespace Atata;

public static class AtataContextBuilderExtensions
{
    public static AtataContextBuilder SetUpWebDrivers(this AtataContextBuilder builder, params string[] browserNames)
    {
        builder.EventSubscriptions.Add(new SetUpWebDriversEventHandler(browserNames));
        return builder;
    }

    public static AtataContextBuilder SetUpWebDriversForUse(this AtataContextBuilder builder)
    {
        builder.EventSubscriptions.Add(SetUpWebDriversForUseEventHandler.Instance);
        return builder;
    }

    public static AtataContextBuilder SetUpWebDriversConfigured(this AtataContextBuilder builder)
    {
        builder.EventSubscriptions.Add(SetUpWebDriversConfiguredEventHandler.Instance);
        return builder;
    }
}
