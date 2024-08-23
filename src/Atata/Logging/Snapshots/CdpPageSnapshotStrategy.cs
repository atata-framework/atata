using Newtonsoft.Json.Linq;
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
    public static CdpPageSnapshotStrategy Instance { get; } =
        new CdpPageSnapshotStrategy();

    /// <inheritdoc/>
    public FileContentWithExtension TakeSnapshot(WebDriverSession session)
    {
        var devTools = session.Driver.As<IDevTools>();
        var devToolsSession = devTools.GetDevToolsSession();

        var commandResult = devToolsSession.SendCommand(
            "Page.captureSnapshot",
            new JObject())
            .GetAwaiter()
            .GetResult();

        var data = commandResult["data"].ToString();
        return FileContentWithExtension.CreateFromText(data, ".mhtml");
    }
}
