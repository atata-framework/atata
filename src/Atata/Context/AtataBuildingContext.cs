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

        public List<Action> CleanUpActions { get; private set; } = new List<Action>();

        /// <summary>
        /// Gets the retry timeout. The default value is 5 seconds.
        /// </summary>
        public TimeSpan RetryTimeout { get; internal set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Gets the retry interval. The default value is 500 milliseconds.
        /// </summary>
        public TimeSpan RetryInterval { get; internal set; } = TimeSpan.FromSeconds(0.5);

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

            return copy;
        }
    }
}
