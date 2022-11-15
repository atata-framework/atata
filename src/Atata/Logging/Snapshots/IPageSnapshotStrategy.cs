namespace Atata
{
    /// <summary>
    /// A strategy that takes a page snapshot.
    /// </summary>
    public interface IPageSnapshotStrategy
    {
        /// <summary>
        /// Takes the snapshot.
        /// </summary>
        /// <param name="context">The <see cref="AtataContext"/> instance.</param>
        /// <returns>The snapshot file content with extension for further saving.</returns>
        FileContentWithExtension TakeSnapshot(AtataContext context);
    }
}
