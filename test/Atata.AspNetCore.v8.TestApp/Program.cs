var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/ping", (ILogger<Program> logger) =>
{
    logger.LogTrace("trace message");
    logger.LogDebug("debug message");
    logger.LogInformation("information message");
    logger.LogWarning("warning message");
    logger.LogError("error message");
    logger.LogCritical("critical message");
    return "pong";
});

await app.RunAsync();

public partial class Program
{
}
