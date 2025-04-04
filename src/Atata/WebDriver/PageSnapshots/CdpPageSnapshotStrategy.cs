using System.Text.Json.Nodes;
using OpenQA.Selenium.DevTools;

namespace Atata;

/// <summary>
/// Represents a <see cref="WebDriverSession"/> page snapshot strategy that takes a page snapshot using CDP.
/// </summary>
public sealed class CdpPageSnapshotStrategy : IPageSnapshotStrategy<WebDriverSession>
{
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static CdpPageSnapshotStrategy Instance { get; } = new();

    /// <inheritdoc/>
    public FileContentWithExtension TakeSnapshot(WebDriverSession session)
    {
        var devTools = session.Driver.As<IDevTools>();
        var devToolsSession = devTools.GetDevToolsSession();

        var commandResult = devToolsSession.SendCommand(
            "Page.captureSnapshot",
            new JsonObject())
            .RunSync();

        string data = commandResult!.Value.GetProperty("data").GetString()!;
        return FileContentWithExtension.CreateFromText(data, ".mhtml");
    }
}
