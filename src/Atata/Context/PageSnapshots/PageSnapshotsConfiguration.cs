using System;

namespace Atata
{
    /// <summary>
    /// Represents the configuration of page snapshots functionality.
    /// </summary>
    public sealed class PageSnapshotsConfiguration : ICloneable
    {
        /// <summary>
        /// Gets or sets the strategy for a page snapshot taking.
        /// The default value is an instance of <see cref="CdpOrPageSourcePageSnapshotStrategy"/>.
        /// </summary>
        public IPageSnapshotStrategy Strategy { get; set; } = CdpOrPageSourcePageSnapshotStrategy.Instance;

        /// <summary>
        /// Gets or sets the page snapshot file name template.
        /// The file name is relative to Artifacts path.
        /// The default value is <c>"{snapshot-number:D2} - {snapshot-pageobjectname} {snapshot-pageobjecttypename}{snapshot-title: - *}"</c>.
        /// </summary>
        public string FileNameTemplate { get; set; } =
            "{snapshot-number:D2} - {snapshot-pageobjectname} {snapshot-pageobjecttypename}{snapshot-title: - *}";

        object ICloneable.Clone() => Clone();

        internal PageSnapshotsConfiguration Clone()
        {
            PageSnapshotsConfiguration clone = (PageSnapshotsConfiguration)MemberwiseClone();

            if (Strategy is ICloneable cloneablePageSnapshotStrategy)
                clone.Strategy = (IPageSnapshotStrategy)cloneablePageSnapshotStrategy.Clone();

            return clone;
        }
    }
}
