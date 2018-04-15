using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the building context for <see cref="AtataContext"/> creation.
    /// It is used by <see cref="AtataContextBuilder"/>.
    /// </summary>
    public class AtataBuildingContext : ICloneable
    {
        private TimeSpan? elementFindTimeout;

        private TimeSpan? elementFindRetryInterval;

        private TimeSpan? waitingTimeout;

        private TimeSpan? waitingRetryInterval;

        private TimeSpan? verificationTimeout;

        private TimeSpan? verificationRetryInterval;

        internal AtataBuildingContext()
        {
        }

        /// <summary>
        /// Gets the driver factories.
        /// </summary>
        public List<IDriverFactory> DriverFactories { get; private set; } = new List<IDriverFactory>();

        /// <summary>
        /// Gets the log consumers.
        /// </summary>
        public List<LogConsumerInfo> LogConsumers { get; private set; } = new List<LogConsumerInfo>();

        /// <summary>
        /// Gets the screenshot consumers.
        /// </summary>
        public List<IScreenshotConsumer> ScreenshotConsumers { get; private set; } = new List<IScreenshotConsumer>();

        /// <summary>
        /// Gets the driver factory to use.
        /// </summary>
        public IDriverFactory DriverFactoryToUse { get; internal set; }

        /// <summary>
        /// Gets or sets the factory method of the test name.
        /// </summary>
        public Func<string> TestNameFactory { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets the actions to perform during <see cref="AtataContext"/> building.
        /// </summary>
        public List<Action> OnBuildingActions { get; private set; } = new List<Action>();

        /// <summary>
        /// Gets the actions to perform after <see cref="AtataContext"/> building.
        /// </summary>
        public List<Action> OnBuiltActions { get; private set; } = new List<Action>();

        /// <summary>
        /// Gets the actions to perform during <see cref="AtataContext"/> cleanup.
        /// </summary>
        public List<Action> CleanUpActions { get; private set; } = new List<Action>();

        /// <summary>
        /// Gets the base retry timeout. The default value is 5 seconds.
        /// </summary>
        [Obsolete("Use BaseRetryTimeout instead.")] // Obsolete since v0.17.0.
        public TimeSpan RetryTimeout => BaseRetryTimeout;

        /// <summary>
        /// Gets the base retry interval. The default value is 500 milliseconds.
        /// </summary>
        [Obsolete("Use BaseRetryInterval instead.")] // Obsolete since v0.17.0.
        public TimeSpan RetryInterval => BaseRetryInterval;

        /// <summary>
        /// Gets the base retry timeout. The default value is 5 seconds.
        /// </summary>
        public TimeSpan BaseRetryTimeout { get; internal set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Gets the base retry interval. The default value is 500 milliseconds.
        /// </summary>
        public TimeSpan BaseRetryInterval { get; internal set; } = TimeSpan.FromSeconds(0.5);

        /// <summary>
        /// Gets the element find timeout.
        /// The default value is taken from <see cref="BaseRetryTimeout"/>, which is equal to 5 seconds by default.
        /// </summary>
        public TimeSpan ElementFindTimeout
        {
            get => elementFindTimeout ?? BaseRetryTimeout;
            internal set => elementFindTimeout = value;
        }

        /// <summary>
        /// Gets the element find retry interval.
        /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to 500 milliseconds by default.
        /// </summary>
        public TimeSpan ElementFindRetryInterval
        {
            get => elementFindRetryInterval ?? BaseRetryInterval;
            internal set => elementFindRetryInterval = value;
        }

        /// <summary>
        /// Gets the waiting timeout.
        /// The default value is taken from <see cref="BaseRetryTimeout"/>, which is equal to 5 seconds by default.
        /// </summary>
        public TimeSpan WaitingTimeout
        {
            get => waitingTimeout ?? BaseRetryTimeout;
            internal set => waitingTimeout = value;
        }

        /// <summary>
        /// Gets the waiting retry interval.
        /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to 500 milliseconds by default.
        /// </summary>
        public TimeSpan WaitingRetryInterval
        {
            get => waitingRetryInterval ?? BaseRetryInterval;
            internal set => waitingRetryInterval = value;
        }

        /// <summary>
        /// Gets the verification timeout.
        /// The default value is taken from <see cref="BaseRetryTimeout"/>, which is equal to 5 seconds by default.
        /// </summary>
        public TimeSpan VerificationTimeout
        {
            get => verificationTimeout ?? BaseRetryTimeout;
            internal set => verificationTimeout = value;
        }

        /// <summary>
        /// Gets the verification retry interval.
        /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to 500 milliseconds by default.
        /// </summary>
        public TimeSpan VerificationRetryInterval
        {
            get => verificationRetryInterval ?? BaseRetryInterval;
            internal set => verificationRetryInterval = value;
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets the type of the assertion exception. The default value is typeof(Atata.AssertionException).
        /// </summary>
        public Type AssertionExceptionType { get; set; } = typeof(AssertionException);

        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Creates a copy of the current instance.
        /// </summary>
        /// <returns>The copied <see cref="AtataBuildingContext"/> instance.</returns>
        public AtataBuildingContext Clone()
        {
            AtataBuildingContext copy = (AtataBuildingContext)MemberwiseClone();

            copy.DriverFactories = DriverFactories.ToList();
            copy.LogConsumers = LogConsumers.ToList();
            copy.ScreenshotConsumers = ScreenshotConsumers.ToList();
            copy.CleanUpActions = CleanUpActions.ToList();
            copy.OnBuildingActions = OnBuildingActions.ToList();
            copy.OnBuiltActions = OnBuiltActions.ToList();

            return copy;
        }
    }
}
