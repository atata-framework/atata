using System.Text.Json.Nodes;
using OpenQA.Selenium.DevTools;

namespace Atata;

/// <summary>
/// Represents the strategy that takes a page snapshot using CDP.
/// </summary>
public sealed class CdpPageSnapshotStrategy : IPageSnapshotStrategy
{
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static CdpPageSnapshotStrategy Instance { get; } =
        new CdpPageSnapshotStrategy();

    /// <inheritdoc/>
    public FileContentWithExtension TakeSnapshot(AtataContext context)
    {
        var devTools = context.Driver.As<IDevTools>();
        var devToolsSession = devTools.GetDevToolsSession();

        var commandResult = devToolsSession.SendCommand(
            "Page.captureSnapshot",
            new JsonObject())
            .GetAwaiter()
            .GetResult();

        string data = commandResult.Value.GetProperty("data").GetString();
        return FileContentWithExtension.CreateFromText(data, ".mhtml");
    }
}
