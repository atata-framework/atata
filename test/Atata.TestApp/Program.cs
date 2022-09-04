namespace Atata.TestApp;

public static class Program
{
    public static void Main(string[] args) =>
        CreateWebApplication(new WebApplicationOptions { Args = args })
            .Run();

    public static WebApplication CreateWebApplication(WebApplicationOptions options)
    {
        var builder = WebApplication.CreateBuilder(options);

        builder.Services.AddRazorPages();

        var app = builder.Build();

        app.UseDeveloperExceptionPage();
        app.UseStatusCodePages();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapRazorPages();

        return app;
    }
}
