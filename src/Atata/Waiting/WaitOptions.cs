namespace Atata
{
    /// <summary>
    /// Represents the component waiting options.
    /// </summary>
    public class WaitOptions
    {
        private double? presenceTimeout;

        private double? absenceTimeout;

        private double? retryInterval;

        public WaitOptions(double? timeout = null)
        {
            presenceTimeout = timeout;
            absenceTimeout = timeout;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to throw the exception on the presence (exists or visible) failure. The default value is true.
        /// </summary>
        public bool ThrowOnPresenceFailure { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to throw the exception on the absence (missing or hidden) failure. The default value is true.
        /// </summary>
        public bool ThrowOnAbsenceFailure { get; set; } = true;

        /// <summary>
        /// Gets or sets the presence (exists or visible) timeout in seconds. The default value is taken from <c>AtataContext.Current.RetryTimeout.TotalSeconds</c>.
        /// </summary>
        public double PresenceTimeout
        {
            get => presenceTimeout ?? (AtataContext.Current?.RetryTimeout ?? RetrySettings.Timeout).TotalSeconds;
            set => presenceTimeout = value;
        }

        /// <summary>
        /// Gets or sets the absence (missing or hidden) timeout in seconds. The default value is taken from <c>AtataContext.Current.RetryTimeout.TotalSeconds</c>.
        /// </summary>
        public double AbsenceTimeout
        {
            get => absenceTimeout ?? (AtataContext.Current?.RetryTimeout ?? RetrySettings.Timeout).TotalSeconds;
            set => absenceTimeout = value;
        }

        /// <summary>
        /// Gets or sets the retry interval. The default value is taken from <c>AtataContext.Current.RetryInterval.TotalSeconds</c>.
        /// </summary>
        public double RetryInterval
        {
            get => retryInterval ?? (AtataContext.Current?.RetryInterval ?? RetrySettings.Interval).TotalSeconds;
            set => retryInterval = value;
        }
    }
}
