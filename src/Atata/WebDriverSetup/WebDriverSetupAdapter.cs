namespace Atata;

internal static class WebDriverSetupAdapter
{
    internal static async Task AutoSetUpSafelyAsync(IEnumerable<string> browserNames, CancellationToken cancellationToken)
    {
        Type driverSetupType = Type.GetType("Atata.WebDriverSetup.DriverSetup,Atata.WebDriverSetup", true);

        var setUpMethod = driverSetupType.GetMethodWithThrowOnError(
            "AutoSetUpSafelyAsync",
            BindingFlags.Public | BindingFlags.Static);

        var task = (Task)setUpMethod.InvokeWithExceptionUnwrapping(null, [browserNames, cancellationToken])!;
        await task.ConfigureAwait(false);
    }
}
