using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium.Remote;

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
        /// Gets the list of secret strings to mask in log.
        /// </summary>
        public List<SecretStringToMask> SecretStringsToMaskInLog { get; private set; } = new List<SecretStringToMask>();

        /// <summary>
        /// Gets the screenshot consumers.
        /// </summary>
        public List<IScreenshotConsumer> ScreenshotConsumers { get; private set; } = new List<IScreenshotConsumer>();

        /// <summary>
        /// Gets the driver factory to use.
        /// </summary>
        public IDriverFactory DriverFactoryToUse { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether it uses a local browser.
        /// Basically, determines whether <see cref="DriverFactoryToUse"/> is <see cref="IUsesLocalBrowser"/>.
        /// </summary>
        public bool UsesLocalBrowser =>
            DriverFactoryToUse is IUsesLocalBrowser;

        /// <summary>
        /// Gets the name of the local browser to use or <see langword="null"/>.
        /// Returns <see cref="IUsesLocalBrowser.BrowserName"/> value if <see cref="DriverFactoryToUse"/> is <see cref="IUsesLocalBrowser"/>.
        /// </summary>
        public string LocalBrowserToUseName =>
            (DriverFactoryToUse as IUsesLocalBrowser)?.BrowserName;

        /// <summary>
        /// Gets the names of local browsers that this instance uses.
        /// Distinctly returns <see cref="IUsesLocalBrowser.BrowserName"/> values of all <see cref="DriverFactories"/> that are <see cref="IUsesLocalBrowser"/>.
        /// </summary>
        public IEnumerable<string> ConfiguredLocalBrowserNames =>
            DriverFactories.OfType<IUsesLocalBrowser>().Select(x => x.BrowserName).Distinct();

        /// <summary>
        /// Gets or sets the factory method of the test name.
        /// </summary>
        public Func<string> TestNameFactory { get; set; }

        /// <summary>
        /// Gets or sets the factory method of the test fixture name.
        /// </summary>
        public Func<string> TestFixtureNameFactory { get; set; }

        /// <summary>
        /// Gets or sets the factory method of the test fixture type.
        /// </summary>
        public Func<Type> TestFixtureTypeFactory { get; set; }

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
        /// Gets the actions to perform after the driver is created.
        /// </summary>
        public List<Action<RemoteWebDriver>> OnDriverCreatedActions { get; private set; } = new List<Action<RemoteWebDriver>>();

        /// <summary>
        /// Gets the actions to perform during <see cref="AtataContext"/> cleanup.
        /// </summary>
        public List<Action> CleanUpActions { get; private set; } = new List<Action>();

        /// <summary>
        /// Gets the context of the attributes.
        /// </summary>
        public AtataAttributesContext Attributes { get; private set; } = new AtataAttributesContext();

        /// <summary>
        /// Gets or sets the default assembly name pattern that is used to filter assemblies to find types in them.
        /// The default value is <c>@"^(?!System($|\..+$)|mscorlib$|netstandard$|Microsoft\..+)"</c>, which filters non-system assemblies.
        /// </summary>
        public string DefaultAssemblyNamePatternToFindTypes { get; set; } = @"^(?!System($|\..+)|mscorlib$|netstandard$|Microsoft\..+)";

        /// <summary>
        /// Gets or sets the assembly name pattern that is used to filter assemblies to find component types in them.
        /// The default value is <see langword="null"/>, which means to use <see cref="DefaultAssemblyNamePatternToFindTypes"/>.
        /// </summary>
        public string AssemblyNamePatternToFindComponentTypes { get; set; }

        /// <summary>
        /// Gets or sets the assembly name pattern that is used to filter assemblies to find attribute types in them.
        /// The default value is <see langword="null"/>, which means to use <see cref="DefaultAssemblyNamePatternToFindTypes"/>.
        /// </summary>
        public string AssemblyNamePatternToFindAttributeTypes { get; set; }

        /// <summary>
        /// Gets the base retry timeout.
        /// The default value is <c>5</c> seconds.
        /// </summary>
        [Obsolete("Use BaseRetryTimeout instead.")] // Obsolete since v0.17.0.
        public TimeSpan RetryTimeout => BaseRetryTimeout;

        /// <summary>
        /// Gets the base retry interval.
        /// The default value is <c>500</c> milliseconds.
        /// </summary>
        [Obsolete("Use BaseRetryInterval instead.")] // Obsolete since v0.17.0.
        public TimeSpan RetryInterval => BaseRetryInterval;

        /// <summary>
        /// Gets the base retry timeout.
        /// The default value is <c>5</c> seconds.
        /// </summary>
        public TimeSpan BaseRetryTimeout { get; internal set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Gets the base retry interval.
        /// The default value is <c>500</c> milliseconds.
        /// </summary>
        public TimeSpan BaseRetryInterval { get; internal set; } = TimeSpan.FromSeconds(0.5);

        /// <summary>
        /// Gets the element find timeout.
        /// The default value is taken from <see cref="BaseRetryTimeout"/>, which is equal to <c>5</c> seconds by default.
        /// </summary>
        public TimeSpan ElementFindTimeout
        {
            get => elementFindTimeout ?? BaseRetryTimeout;
            internal set => elementFindTimeout = value;
        }

        /// <summary>
        /// Gets the element find retry interval.
        /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to <c>500</c> milliseconds by default.
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
        /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to <c>500</c> milliseconds by default.
        /// </summary>
        public TimeSpan WaitingRetryInterval
        {
            get => waitingRetryInterval ?? BaseRetryInterval;
            internal set => waitingRetryInterval = value;
        }

        /// <summary>
        /// Gets the verification timeout.
        /// The default value is taken from <see cref="BaseRetryTimeout"/>, which is equal to <c>5</c> seconds by default.
        /// </summary>
        public TimeSpan VerificationTimeout
        {
            get => verificationTimeout ?? BaseRetryTimeout;
            internal set => verificationTimeout = value;
        }

        /// <summary>
        /// Gets the verification retry interval.
        /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to <c>500</c> milliseconds by default.
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
        /// Gets or sets the type of the assertion exception.
        /// The default value is a type of <see cref="AssertionException"/>.
        /// </summary>
        public Type AssertionExceptionType { get; set; } = typeof(AssertionException);

        /// <summary>
        /// Gets or sets the type of the aggregate assertion exception.
        /// The default value is a type of <see cref="AggregateAssertionException"/>.
        /// The exception type should have public constructor with <c>IEnumerable&lt;AssertionResult&gt;</c> argument.
        /// </summary>
        public Type AggregateAssertionExceptionType { get; set; } = typeof(AggregateAssertionException);

        /// <summary>
        /// Gets or sets the aggregate assertion strategy.
        /// The default value is an instance of <see cref="AtataAggregateAssertionStrategy"/>.
        /// </summary>
        public IAggregateAssertionStrategy AggregateAssertionStrategy { get; set; } = new AtataAggregateAssertionStrategy();

        /// <summary>
        /// Gets or sets the strategy for warning assertion reporting.
        /// The default value is an instance of <see cref="AtataWarningReportStrategy"/>.
        /// </summary>
        public IWarningReportStrategy WarningReportStrategy { get; set; } = new AtataWarningReportStrategy();

        /// <summary>
        /// Gets the driver factory by the specified alias.
        /// </summary>
        /// <param name="alias">The alias of the driver factory.</param>
        /// <returns>The driver factory or <see langword="null"/>.</returns>
        public IDriverFactory GetDriverFactory(string alias)
        {
            alias.CheckNotNullOrWhitespace(nameof(alias));

            return DriverFactories.LastOrDefault(x => alias.Equals(x.Alias, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc cref="Clone"/>
        object ICloneable.Clone() =>
            Clone();

        /// <summary>
        /// Creates a copy of the current instance.
        /// </summary>
        /// <returns>The copied <see cref="AtataBuildingContext"/> instance.</returns>
        public AtataBuildingContext Clone()
        {
            AtataBuildingContext copy = (AtataBuildingContext)MemberwiseClone();

            copy.DriverFactories = DriverFactories.ToList();
            copy.LogConsumers = LogConsumers.ToList();
            copy.SecretStringsToMaskInLog = SecretStringsToMaskInLog.ToList();
            copy.ScreenshotConsumers = ScreenshotConsumers.ToList();
            copy.OnBuildingActions = OnBuildingActions.ToList();
            copy.OnBuiltActions = OnBuiltActions.ToList();
            copy.OnDriverCreatedActions = OnDriverCreatedActions.ToList();
            copy.CleanUpActions = CleanUpActions.ToList();
            copy.Attributes = Attributes.Clone();

            return copy;
        }
    }
}
