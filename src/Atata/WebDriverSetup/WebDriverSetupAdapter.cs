namespace Atata;

internal static class WebDriverSetupAdapter
{
    internal static async Task AutoSetUpSafelyAsync(IEnumerable<string> browserNames)
    {
        Type driverSetupType = Type.GetType("Atata.WebDriverSetup.DriverSetup,Atata.WebDriverSetup", true);

        var setUpMethod = driverSetupType.GetMethodWithThrowOnError(
            "AutoSetUpSafelyAsync",
            BindingFlags.Public | BindingFlags.Static);

        Task task = setUpMethod.InvokeStaticAsLambda<Task>(browserNames);
        await task.ConfigureAwait(false);
    }
}
