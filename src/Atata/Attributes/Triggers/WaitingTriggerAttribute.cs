namespace Atata
{
    /// <summary>
    /// Represents the base trigger attribute for a waiting functionality.
    /// </summary>
    public abstract class WaitingTriggerAttribute : TriggerAttribute
    {
        private double? timeout;

        private double? retryInterval;

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
            get => timeout ?? (AtataContext.Current?.WaitingTimeout ?? RetrySettings.Timeout).TotalSeconds;
            set => timeout = value;
        }

        /// <summary>
        /// Gets or sets the retry interval in seconds.
        /// The default value is taken from <see cref="AtataContext.WaitingRetryInterval"/> property of <see cref="AtataContext.Current"/>.
        /// </summary>
        public double RetryInterval
        {
            get => retryInterval ?? (AtataContext.Current?.WaitingRetryInterval ?? RetrySettings.Interval).TotalSeconds;
            set => retryInterval = value;
        }
    }
}
