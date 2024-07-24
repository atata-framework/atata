namespace Atata;

/// <summary>
/// A strategy that takes a page snapshot.
/// </summary>
public interface IPageSnapshotStrategy
{
    /// <summary>
    /// Takes the snapshot.
    /// </summary>
    /// <param name="session">The <see cref="WebDriverSession"/> instance.</param>
    /// <returns>The snapshot file content with extension for further saving.</returns>
    FileContentWithExtension TakeSnapshot(WebDriverSession session);
}
