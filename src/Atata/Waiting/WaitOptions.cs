namespace Atata
{
    /// <summary>
    /// Represents the component waiting options.
    /// </summary>
    public class WaitOptions
    {
        private double? _presenceTimeout;

        private double? _absenceTimeout;

        private double? _retryInterval;

        public WaitOptions(double? timeout = null)
        {
            _presenceTimeout = timeout;
            _absenceTimeout = timeout;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to throw the exception on the presence (exists or visible) failure.
        /// The default value is <see langword="true"/>.
        /// </summary>
        public bool ThrowOnPresenceFailure { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to throw the exception on the absence (missing or hidden) failure.
        /// The default value is <see langword="true"/>.
        /// </summary>
        public bool ThrowOnAbsenceFailure { get; set; } = true;

        /// <summary>
        /// Gets or sets the presence (exists or visible) timeout in seconds.
        /// The default value is taken from <c>AtataContext.Current.WaitingTimeout.TotalSeconds</c>.
        /// </summary>
        public double PresenceTimeout
        {
            get => _presenceTimeout ?? (AtataContext.Current?.WaitingTimeout ?? RetrySettings.Timeout).TotalSeconds;
            set => _presenceTimeout = value;
        }

        /// <summary>
        /// Gets or sets the absence (missing or hidden) timeout in seconds.
        /// The default value is taken from <c>AtataContext.Current.WaitingTimeout.TotalSeconds</c>.
        /// </summary>
        public double AbsenceTimeout
        {
            get => _absenceTimeout ?? (AtataContext.Current?.WaitingTimeout ?? RetrySettings.Timeout).TotalSeconds;
            set => _absenceTimeout = value;
        }

        /// <summary>
        /// Gets or sets the retry interval in seconds.
        /// The default value is taken from <c>AtataContext.Current.WaitingRetryInterval.TotalSeconds</c>.
        /// </summary>
        public double RetryInterval
        {
            get => _retryInterval ?? (AtataContext.Current?.WaitingRetryInterval ?? RetrySettings.Interval).TotalSeconds;
            set => _retryInterval = value;
        }
    }
}
