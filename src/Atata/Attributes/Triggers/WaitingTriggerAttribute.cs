namespace Atata
{
    /// <summary>
    /// Represents the base trigger attribute for a waiting functionality.
    /// </summary>
    public abstract class WaitingTriggerAttribute : TriggerAttribute
    {
        private double? _timeout;

        private double? _retryInterval;

        protected WaitingTriggerAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        /// <summary>
        /// Gets or sets the waiting timeout in seconds.
        /// The default value is taken from <see cref="AtataContext.WaitingTimeout"/> property of <see cref="AtataContext.Current"/>.
        /// </summary>
        public double Timeout
        {
            get => _timeout ?? (AtataContext.Current?.WaitingTimeout ?? RetrySettings.Timeout).TotalSeconds;
            set => _timeout = value;
        }

        /// <summary>
        /// Gets or sets the retry interval in seconds.
        /// The default value is taken from <see cref="AtataContext.WaitingRetryInterval"/> property of <see cref="AtataContext.Current"/>.
        /// </summary>
        public double RetryInterval
        {
            get => _retryInterval ?? (AtataContext.Current?.WaitingRetryInterval ?? RetrySettings.Interval).TotalSeconds;
            set => _retryInterval = value;
        }
    }
}
