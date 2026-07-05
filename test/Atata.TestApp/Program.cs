using System.Diagnostics.CodeAnalysis;

namespace Atata.TestApp;

public static class Program
{
    public static void Main(string[] args) =>
        CreateWebApplication(new() { Args = args })
            .Run();

    [SuppressMessage("Minor Vulnerability", "S4507:Debugging features should not be enabled in production")]
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
