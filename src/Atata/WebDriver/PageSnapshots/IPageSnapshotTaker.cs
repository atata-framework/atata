namespace Atata;

/// <summary>
/// Provides a page snapshot taking method.
/// </summary>
public interface IPageSnapshotTaker
{
    /// <summary>
    /// Takes a snapshot (HTML or MHTML file) of the current page with an optionally specified title.
    /// </summary>
    /// <param name="title">The title of a snapshot.</param>
    void TakeSnapshot(string? title = null);
}
