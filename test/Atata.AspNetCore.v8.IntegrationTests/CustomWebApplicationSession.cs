namespace Atata.AspNetCore.IntegrationTests;

public sealed class CustomWebApplicationSession : WebApplicationSession
{
    public int CallsCountOfConfigureWebHost { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        CallsCountOfConfigureWebHost++;

        builder.ConfigureTestServices(services =>
        {
            // Custom service configuration can be done here.
        });
    }
}
