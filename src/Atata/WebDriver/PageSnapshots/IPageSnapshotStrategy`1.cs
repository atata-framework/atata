#nullable enable

namespace Atata;

/// <summary>
/// A strategy that takes a page snapshot.
/// </summary>
/// <typeparam name="TSession">The type of the session.</typeparam>
public interface IPageSnapshotStrategy<in TSession>
    where TSession : AtataSession
{
    /// <summary>
    /// Takes the snapshot.
    /// </summary>
    /// <param name="session">The <typeparamref name="TSession"/> instance.</param>
    /// <returns>The snapshot file content with extension for further saving.</returns>
    FileContentWithExtension TakeSnapshot(TSession session);
}
