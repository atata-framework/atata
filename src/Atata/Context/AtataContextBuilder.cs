namespace Atata;

/// <summary>
/// Represents the builder of <see cref="AtataContext"/>.
/// </summary>
public class AtataContextBuilder
{
    public AtataContextBuilder(AtataBuildingContext buildingContext)
    {
        BuildingContext = buildingContext.CheckNotNull(nameof(buildingContext));
        Sessions = new AtataSessionsBuilder(this, []);
    }

    /// <summary>
    /// Gets the building context.
    /// </summary>
    public AtataBuildingContext BuildingContext { get; internal set; }

    /// <summary>
    /// Gets the builder of context attributes,
    /// which provides the functionality to add extra attributes to different metadata levels:
    /// global, assembly, component and property.
    /// </summary>
    public AttributesAtataContextBuilder Attributes => new(BuildingContext);

    /// <summary>
    /// Gets the builder of event subscriptions,
    /// which provides the methods to subscribe to Atata and custom events.
    /// </summary>
    public EventSubscriptionsAtataContextBuilder EventSubscriptions => new(BuildingContext);

    /// <summary>
    /// Gets the builder of log consumers,
    /// which provides the methods to add log consumers.
    /// </summary>
    public LogConsumersAtataContextBuilder LogConsumers => new(BuildingContext);

    public AtataSessionsBuilder Sessions { get; }

    /// <summary>
    /// Adds the variable.
    /// </summary>
    /// <param name="key">The variable key.</param>
    /// <param name="value">The variable value.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder AddVariable(string key, object value)
    {
        key.CheckNotNullOrWhitespace(nameof(key));

        BuildingContext.Variables[key] = value;

        return this;
    }

    /// <summary>
    /// Adds the variables.
    /// </summary>
    /// <param name="variables">The variables to add.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder AddVariables(IDictionary<string, object> variables)
    {
        variables.CheckNotNull(nameof(variables));

        foreach (var variable in variables)
            BuildingContext.Variables[variable.Key] = variable.Value;

        return this;
    }

    /// <summary>
    /// Adds the secret string to mask in log.
    /// </summary>
    /// <param name="value">The secret string value.</param>
    /// <param name="mask">The mask, which should replace the secret string.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder AddSecretStringToMaskInLog(string value, string mask = "{*****}")
    {
        value.CheckNotNullOrWhitespace(nameof(value));
        mask.CheckNotNullOrWhitespace(nameof(mask));

        BuildingContext.SecretStringsToMaskInLog.Add(
            new SecretStringToMask(value, mask));

        return this;
    }

    /// <summary>
    /// Sets the factory method of the test name.
    /// </summary>
    /// <param name="testNameFactory">The factory method of the test name.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestName(Func<string> testNameFactory)
    {
        testNameFactory.CheckNotNull(nameof(testNameFactory));

        BuildingContext.TestNameFactory = testNameFactory;
        return this;
    }

    /// <summary>
    /// Sets the name of the test.
    /// </summary>
    /// <param name="testName">The name of the test.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestName(string testName)
    {
        BuildingContext.TestNameFactory = () => testName;
        return this;
    }

    /// <summary>
    /// Sets the factory method of the test suite (fixture/class) name.
    /// </summary>
    /// <param name="testSuiteNameFactory">The factory method of the test suite name.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteName(Func<string> testSuiteNameFactory)
    {
        testSuiteNameFactory.CheckNotNull(nameof(testSuiteNameFactory));

        BuildingContext.TestSuiteNameFactory = testSuiteNameFactory;
        return this;
    }

    /// <summary>
    /// Sets the name of the test suite (fixture/class).
    /// </summary>
    /// <param name="testSuiteName">The name of the test suite.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteName(string testSuiteName)
    {
        BuildingContext.TestSuiteNameFactory = () => testSuiteName;
        return this;
    }

    /// <summary>
    /// Sets the factory method of the test suite (fixture/class) type.
    /// </summary>
    /// <param name="testSuiteTypeFactory">The factory method of the test suite type.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteType(Func<Type> testSuiteTypeFactory)
    {
        testSuiteTypeFactory.CheckNotNull(nameof(testSuiteTypeFactory));

        BuildingContext.TestSuiteTypeFactory = testSuiteTypeFactory;
        return this;
    }

    /// <summary>
    /// Sets the type of the test suite (fixture/class).
    /// </summary>
    /// <param name="testSuiteType">The type of the test suite.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteType(Type testSuiteType)
    {
        testSuiteType.CheckNotNull(nameof(testSuiteType));

        BuildingContext.TestSuiteTypeFactory = () => testSuiteType;
        return this;
    }

    [Obsolete("Use AtataContext.GlobalProperties.UseUtcTimeZone() method instead.")] // Obsolete since v3.0.0.
    public AtataContextBuilder UseUtcTimeZone() =>
        UseTimeZone(TimeZoneInfo.Utc);

    [Obsolete("Use AtataContext.GlobalProperties.UseTimeZone(...) method instead.")] // Obsolete since v3.0.0.
    public AtataContextBuilder UseTimeZone(string timeZoneId)
    {
        timeZoneId.CheckNotNullOrWhitespace(nameof(timeZoneId));
        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        return UseTimeZone(timeZone);
    }

    [Obsolete("Use AtataContext.GlobalProperties.UseTimeZone(...) method instead.")] // Obsolete since v3.0.0.
    public AtataContextBuilder UseTimeZone(TimeZoneInfo timeZone)
    {
        timeZone.CheckNotNull(nameof(timeZone));

        AtataContext.GlobalProperties.UseTimeZone(timeZone);
        return this;
    }

    /// <summary>
    /// Sets the base retry timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseBaseRetryTimeout(TimeSpan timeout)
    {
        BuildingContext.BaseRetryTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Sets the base retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseBaseRetryInterval(TimeSpan interval)
    {
        BuildingContext.BaseRetryInterval = interval;
        return this;
    }

    /// <summary>
    /// Sets the waiting timeout.
    /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryTimeout"/>, which is equal to <c>5</c> seconds by default.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseWaitingTimeout(TimeSpan timeout)
    {
        BuildingContext.WaitingTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Sets the waiting retry interval.
    /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryInterval"/>, which is equal to <c>500</c> milliseconds by default.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseWaitingRetryInterval(TimeSpan interval)
    {
        BuildingContext.WaitingRetryInterval = interval;
        return this;
    }

    /// <summary>
    /// Sets the verification timeout.
    /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryTimeout"/>, which is equal to <c>5</c> seconds by default.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseVerificationTimeout(TimeSpan timeout)
    {
        BuildingContext.VerificationTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Sets the verification retry interval.
    /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryInterval"/>, which is equal to <c>500</c> milliseconds by default.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseVerificationRetryInterval(TimeSpan interval)
    {
        BuildingContext.VerificationRetryInterval = interval;
        return this;
    }

    /// <summary>
    /// Sets the culture.
    /// The default value is <see cref="CultureInfo.CurrentCulture"/>.
    /// </summary>
    /// <param name="culture">The culture.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseCulture(CultureInfo culture)
    {
        BuildingContext.Culture = culture;
        return this;
    }

    /// <summary>
    /// Sets the culture by the name.
    /// The default value is <see cref="CultureInfo.CurrentCulture"/>.
    /// </summary>
    /// <param name="cultureName">The name of the culture.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseCulture(string cultureName) =>
        UseCulture(CultureInfo.GetCultureInfo(cultureName));

    /// <summary>
    /// Sets the type of the assertion exception.
    /// The default value is a type of <see cref="AssertionException"/>.
    /// </summary>
    /// <typeparam name="TException">The type of the exception.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssertionExceptionType<TException>()
        where TException : Exception
        =>
        UseAssertionExceptionType(typeof(TException));

    /// <summary>
    /// Sets the type of the assertion exception.
    /// The default value is a type of <see cref="AssertionException"/>.
    /// </summary>
    /// <param name="exceptionType">The type of the exception.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssertionExceptionType(Type exceptionType)
    {
        exceptionType.CheckIs<Exception>(nameof(exceptionType));

        BuildingContext.AssertionExceptionType = exceptionType;
        return this;
    }

    /// <summary>
    /// Sets the type of aggregate assertion exception.
    /// The default value is a type of <see cref="AggregateAssertionException"/>.
    /// The exception type should have public constructor with <c>IEnumerable&lt;AssertionResult&gt;</c> argument.
    /// </summary>
    /// <typeparam name="TException">The type of the exception.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAggregateAssertionExceptionType<TException>()
        where TException : Exception
        =>
        UseAggregateAssertionExceptionType(typeof(TException));

    /// <summary>
    /// Sets the type of aggregate assertion exception.
    /// The default value is a type of <see cref="AggregateAssertionException"/>.
    /// The exception type should have public constructor with <c>IEnumerable&lt;AssertionResult&gt;</c> argument.
    /// </summary>
    /// <param name="exceptionType">The type of the exception.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAggregateAssertionExceptionType(Type exceptionType)
    {
        exceptionType.CheckIs<Exception>(nameof(exceptionType));

        BuildingContext.AggregateAssertionExceptionType = exceptionType;
        return this;
    }

    /// <summary>
    /// Sets the default assembly name pattern that is used to filter assemblies to find types in them.
    /// Modifies the <see cref="AtataBuildingContext.DefaultAssemblyNamePatternToFindTypes"/> property value of <see cref="BuildingContext"/>.
    /// </summary>
    /// <param name="pattern">The pattern.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseDefaultAssemblyNamePatternToFindTypes(string pattern)
    {
        pattern.CheckNotNullOrWhitespace(nameof(pattern));

        BuildingContext.DefaultAssemblyNamePatternToFindTypes = pattern;
        return this;
    }

    /// <summary>
    /// Sets the assembly name pattern that is used to filter assemblies to find component types in them.
    /// Modifies the <see cref="AtataBuildingContext.AssemblyNamePatternToFindComponentTypes"/> property value of <see cref="BuildingContext"/>.
    /// </summary>
    /// <param name="pattern">The pattern.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssemblyNamePatternToFindComponentTypes(string pattern)
    {
        pattern.CheckNotNullOrWhitespace(nameof(pattern));

        BuildingContext.AssemblyNamePatternToFindComponentTypes = pattern;
        return this;
    }

    /// <summary>
    /// Sets the assembly name pattern that is used to filter assemblies to find attribute types in them.
    /// Modifies the <see cref="AtataBuildingContext.AssemblyNamePatternToFindAttributeTypes"/> property value of <see cref="BuildingContext"/>.
    /// </summary>
    /// <param name="pattern">The pattern.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssemblyNamePatternToFindAttributeTypes(string pattern)
    {
        pattern.CheckNotNullOrWhitespace(nameof(pattern));

        BuildingContext.AssemblyNamePatternToFindAttributeTypes = pattern;
        return this;
    }

    /// <summary>
    /// Sets the assembly name pattern that is used to filter assemblies to find event types in them.
    /// Modifies the <see cref="AtataBuildingContext.AssemblyNamePatternToFindEventTypes"/> property value of <see cref="BuildingContext"/>.
    /// </summary>
    /// <param name="pattern">The pattern.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssemblyNamePatternToFindEventTypes(string pattern)
    {
        pattern.CheckNotNullOrWhitespace(nameof(pattern));

        BuildingContext.AssemblyNamePatternToFindEventTypes = pattern;
        return this;
    }

    /// <summary>
    /// Sets the assembly name pattern that is used to filter assemblies to find event handler types in them.
    /// Modifies the <see cref="AtataBuildingContext.AssemblyNamePatternToFindEventHandlerTypes"/> property value of <see cref="BuildingContext"/>.
    /// </summary>
    /// <param name="pattern">The pattern.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssemblyNamePatternToFindEventHandlerTypes(string pattern)
    {
        pattern.CheckNotNullOrWhitespace(nameof(pattern));

        BuildingContext.AssemblyNamePatternToFindEventHandlerTypes = pattern;
        return this;
    }

    /// <summary>
    /// <para>
    /// Sets the Artifacts directory path template.
    /// The default value is <c>"{test-suite-name-sanitized:/*}{test-name-sanitized:/*}"</c>.
    /// </para>
    /// <para>
    /// List of predefined variables:
    /// </para>
    /// <list type="bullet">
    /// <item><c>{test-name-sanitized}</c></item>
    /// <item><c>{test-name}</c></item>
    /// <item><c>{test-suite-name-sanitized}</c></item>
    /// <item><c>{test-suite-name}</c></item>
    /// <item><c>{test-start}</c></item>
    /// <item><c>{test-start-utc}</c></item>
    /// </list>
    /// </summary>
    /// <param name="directoryPathTemplate">The directory path template.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseArtifactsPathTemplate(string directoryPathTemplate)
    {
        directoryPathTemplate.CheckNotNullOrWhitespace(nameof(directoryPathTemplate));

        BuildingContext.ArtifactsPathTemplate = directoryPathTemplate;
        return this;
    }

    /// <summary>
    /// Defines that the name of the test should be taken from the NUnit test.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitTestName() =>
        UseTestName(NUnitAdapter.GetCurrentTestName);

    /// <summary>
    /// Defines that the name of the test suite should be taken from the NUnit test fixture.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitTestSuiteName() =>
        UseTestSuiteName(NUnitAdapter.GetCurrentTestFixtureName);

    /// <summary>
    /// Defines that the type of the test suite should be taken from the NUnit test fixture.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitTestSuiteType() =>
        UseTestSuiteType(NUnitAdapter.GetCurrentTestFixtureType);

    /// <summary>
    /// Sets <see cref="NUnitAggregateAssertionStrategy"/> as the aggregate assertion strategy.
    /// The <see cref="NUnitAggregateAssertionStrategy"/> uses NUnit's <c>Assert.Multiple</c> method for aggregate assertion.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitAggregateAssertionStrategy() =>
        UseAggregateAssertionStrategy(new NUnitAggregateAssertionStrategy());

    /// <summary>
    /// Sets the aggregate assertion strategy.
    /// </summary>
    /// <typeparam name="TAggregateAssertionStrategy">The type of the aggregate assertion strategy.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAggregateAssertionStrategy<TAggregateAssertionStrategy>()
        where TAggregateAssertionStrategy : IAggregateAssertionStrategy, new()
    {
        IAggregateAssertionStrategy strategy = Activator.CreateInstance<TAggregateAssertionStrategy>();

        return UseAggregateAssertionStrategy(strategy);
    }

    /// <summary>
    /// Sets the aggregate assertion strategy.
    /// </summary>
    /// <param name="strategy">The aggregate assertion strategy.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAggregateAssertionStrategy(IAggregateAssertionStrategy strategy)
    {
        BuildingContext.AggregateAssertionStrategy = strategy;

        return this;
    }

    /// <summary>
    /// Sets <see cref="NUnitWarningReportStrategy"/> as the strategy for warning assertion reporting.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitWarningReportStrategy() =>
        UseWarningReportStrategy(new NUnitWarningReportStrategy());

    /// <summary>
    /// Sets the strategy for warning assertion reporting.
    /// </summary>
    /// <param name="strategy">The warning report strategy.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseWarningReportStrategy(IWarningReportStrategy strategy)
    {
        BuildingContext.WarningReportStrategy = strategy;

        return this;
    }

    /// <summary>
    /// Uses the <see cref="NUnitAssertionFailureReportStrategy"/> as the strategy for assertion failure reporting.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitAssertionFailureReportStrategy() =>
        UseAssertionFailureReportStrategy(NUnitAssertionFailureReportStrategy.Instance);

    /// <summary>
    /// Sets the strategy for assertion failure reporting.
    /// </summary>
    /// <param name="strategy">The assertion failure reporting strategy.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssertionFailureReportStrategy(IAssertionFailureReportStrategy strategy)
    {
        BuildingContext.AssertionFailureReportStrategy = strategy;

        return this;
    }

    /// <summary>
    /// Sets the type of <c>NUnit.Framework.AssertionException</c> as the assertion exception type.
    /// The default value is a type of <see cref="AssertionException"/>.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitAssertionExceptionType() =>
        UseAssertionExceptionType(NUnitAdapter.AssertionExceptionType);

    /// <summary>
    /// Enables all Atata features for NUnit.
    /// Executes the following methods:
    /// <list type="bullet">
    /// <item><see cref="UseNUnitTestName"/></item>
    /// <item><see cref="UseNUnitTestSuiteName"/></item>
    /// <item><see cref="UseNUnitTestSuiteType"/></item>
    /// <item><see cref="UseNUnitAssertionExceptionType"/></item>
    /// <item><see cref="UseNUnitAggregateAssertionStrategy"/></item>
    /// <item><see cref="UseNUnitWarningReportStrategy"/></item>
    /// <item><see cref="UseNUnitAssertionFailureReportStrategy"/></item>
    /// <item><see cref="LogConsumersAtataContextBuilder.AddNUnitTestContext"/> of <see cref="LogConsumers"/> property</item>
    /// <item><see cref="EventSubscriptionsAtataContextBuilder.LogNUnitError"/> of <see cref="EventSubscriptions"/> property</item>
    /// <item><see cref="EventSubscriptionsAtataContextBuilder.TakeScreenshotOnNUnitError(string)"/> of <see cref="EventSubscriptions"/> property</item>
    /// <item><see cref="EventSubscriptionsAtataContextBuilder.TakePageSnapshotOnNUnitError(string)"/> of <see cref="EventSubscriptions"/> property</item>
    /// <item><see cref="EventSubscriptionsAtataContextBuilder.AddArtifactsToNUnitTestContext"/> of <see cref="EventSubscriptions"/> property</item>
    /// </list>
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAllNUnitFeatures() =>
        UseNUnitTestName()
            .UseNUnitTestSuiteName()
            .UseNUnitTestSuiteType()
            .UseNUnitAssertionExceptionType()
            .UseNUnitAggregateAssertionStrategy()
            .UseNUnitWarningReportStrategy()
            .UseNUnitAssertionFailureReportStrategy()
            .LogConsumers.AddNUnitTestContext()
            .EventSubscriptions.LogNUnitError()
            .EventSubscriptions.TakeScreenshotOnNUnitError()
            .EventSubscriptions.TakePageSnapshotOnNUnitError()
            .EventSubscriptions.AddArtifactsToNUnitTestContext();

    /// <summary>
    /// Enables all Atata features for SpecFlow+NUnit.
    /// Executes the following methods:
    /// <list type="bullet">
    /// <item><see cref="UseNUnitTestName"/></item>
    /// <item><see cref="UseNUnitTestSuiteName"/></item>
    /// <item><see cref="UseNUnitTestSuiteType"/></item>
    /// <item><see cref="UseNUnitAssertionExceptionType"/></item>
    /// <item><see cref="UseNUnitAggregateAssertionStrategy"/></item>
    /// <item><see cref="UseNUnitWarningReportStrategy"/></item>
    /// <item><see cref="UseNUnitAssertionFailureReportStrategy"/></item>
    /// <item><see cref="EventSubscriptionsAtataContextBuilder.LogNUnitError"/> of <see cref="EventSubscriptions"/> property</item>
    /// <item><see cref="EventSubscriptionsAtataContextBuilder.TakeScreenshotOnNUnitError(string)"/> of <see cref="EventSubscriptions"/> property</item>
    /// <item><see cref="EventSubscriptionsAtataContextBuilder.TakePageSnapshotOnNUnitError(string)"/> of <see cref="EventSubscriptions"/> property</item>
    /// </list>
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseSpecFlowNUnitFeatures() =>
        UseNUnitTestName()
            .UseNUnitTestSuiteName()
            .UseNUnitTestSuiteType()
            .UseNUnitAssertionExceptionType()
            .UseNUnitAggregateAssertionStrategy()
            .UseNUnitWarningReportStrategy()
            .UseNUnitAssertionFailureReportStrategy()
            .EventSubscriptions.LogNUnitError()
            .EventSubscriptions.TakeScreenshotOnNUnitError()
            .EventSubscriptions.TakePageSnapshotOnNUnitError();

    private DirectorySubject CreateArtifactsDirectorySubject(AtataContext context)
    {
        string pathTemplate = BuildingContext.ArtifactsPathTemplate;

        string path = context.Variables.FillPathTemplateString(pathTemplate);
        string fullPath;

        if (path.Length > 0)
        {
            if (path[0] == Path.DirectorySeparatorChar || path[0] == Path.AltDirectorySeparatorChar)
                path = path.Substring(1);

            fullPath = Path.Combine(AtataContext.GlobalProperties.ArtifactsRootPath, path);
        }
        else
        {
            fullPath = AtataContext.GlobalProperties.ArtifactsRootPath;
        }

        return new DirectorySubject(fullPath, "Artifacts");
    }

    /// <summary>
    /// Clears the <see cref="BuildingContext"/>.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Clear()
    {
        BuildingContext = new AtataBuildingContext();
        return this;
    }

    /// <summary>
    /// Builds the <see cref="AtataContext" /> instance and sets it to <see cref="AtataContext.Current" /> property.
    /// </summary>
    /// <returns>The created <see cref="AtataContext"/> instance.</returns>
    public AtataContext Build()
    {
        AtataContext context = new AtataContext();
        LogManager logManager = CreateLogManager(context);

        IObjectConverter objectConverter = new ObjectConverter
        {
            AssemblyNamePatternToFindTypes = BuildingContext.DefaultAssemblyNamePatternToFindTypes
        };

        IObjectMapper objectMapper = new ObjectMapper(objectConverter);
        IObjectCreator objectCreator = new ObjectCreator(objectConverter, objectMapper);

        context.Test.Name = BuildingContext.TestNameFactory?.Invoke();
        context.Test.SuiteName = BuildingContext.TestSuiteNameFactory?.Invoke();
        context.Test.SuiteType = BuildingContext.TestSuiteTypeFactory?.Invoke();
        context.Log = logManager;
        context.Attributes = BuildingContext.Attributes.Clone();
        context.BaseRetryTimeout = BuildingContext.BaseRetryTimeout;
        context.BaseRetryInterval = BuildingContext.BaseRetryInterval;
        context.WaitingTimeout = BuildingContext.WaitingTimeout;
        context.WaitingRetryInterval = BuildingContext.WaitingRetryInterval;
        context.VerificationTimeout = BuildingContext.VerificationTimeout;
        context.VerificationRetryInterval = BuildingContext.VerificationRetryInterval;
        context.Culture = BuildingContext.Culture ?? CultureInfo.CurrentCulture;
        context.AssertionExceptionType = BuildingContext.AssertionExceptionType;
        context.AggregateAssertionExceptionType = BuildingContext.AggregateAssertionExceptionType;
        context.AggregateAssertionStrategy = BuildingContext.AggregateAssertionStrategy ?? new AtataAggregateAssertionStrategy();
        context.WarningReportStrategy = BuildingContext.WarningReportStrategy ?? new AtataWarningReportStrategy();
        context.AssertionFailureReportStrategy = BuildingContext.AssertionFailureReportStrategy ?? AtataAssertionFailureReportStrategy.Instance;
        context.ObjectConverter = objectConverter;
        context.ObjectMapper = objectMapper;
        context.ObjectCreator = objectCreator;
        context.EventBus = new EventBus(context, BuildingContext.EventSubscriptions);

        if (context.Test.SuiteName is null && context.Test.SuiteType is not null)
            context.Test.SuiteName = context.Test.SuiteType.Name;

        context.InitDateTimeProperties();
        context.InitMainVariables();
        context.InitCustomVariables(BuildingContext.Variables);
        context.Artifacts = CreateArtifactsDirectorySubject(context);
        context.InitArtifactsVariable();

        AtataContext.Current = context;

        context.EventBus.Publish(new AtataContextPreInitEvent(context));

        context.LogTestStart();

        context.Log.ExecuteSection(
            new LogSection("Initialize AtataContext", LogLevel.Trace),
            () => InitializeContext(context));

        context.BodyExecutionStopwatch.Start();

        return context;
    }

    private LogManager CreateLogManager(AtataContext context)
    {
        LogManagerConfiguration configuration = new(
            [.. BuildingContext.LogConsumerConfigurations],
            [.. BuildingContext.SecretStringsToMaskInLog]);

        return new(configuration, new AtataContextLogEventInfoFactory(context));
    }

    private void InitializeContext(AtataContext context)
    {
        context.EventBus.Publish(new AtataContextInitStartedEvent(context));

        if (context.BaseUrl != null)
            context.Log.Trace($"Set: BaseUrl={context.BaseUrl}");

        LogRetrySettings(context);

        if (BuildingContext.Culture != null)
            ApplyCulture(context, BuildingContext.Culture);

        context.Log.Trace($"Set: Artifacts={context.ArtifactsPath}");

        // TODO: Build sessions.

        // TODO: Start sessions that should be start.

        context.EventBus.Publish(new AtataContextInitCompletedEvent(context));
    }

    private static void LogRetrySettings(AtataContext context)
    {
        string messageFormat = "Set: {0}Timeout={1}; {0}RetryInterval={2}";

        context.Log.Trace(
            messageFormat.FormatWith(
                "ElementFind",
                context.ElementFindTimeout.ToShortIntervalString(),
                context.ElementFindRetryInterval.ToShortIntervalString()));

        context.Log.Trace(
            messageFormat.FormatWith(
                "Waiting",
                context.WaitingTimeout.ToShortIntervalString(),
                context.WaitingRetryInterval.ToShortIntervalString()));

        context.Log.Trace(
            messageFormat.FormatWith(
                "Verification",
                context.VerificationTimeout.ToShortIntervalString(),
                context.VerificationRetryInterval.ToShortIntervalString()));
    }

    private static void ApplyCulture(AtataContext context, CultureInfo culture)
    {
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = culture;

        if (AtataContext.GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.Static)
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = culture;

        context.Log.Trace($"Set: Culture={culture.Name}");
    }

    protected internal IObjectMapper CreateObjectMapper()
    {
        IObjectConverter objectConverter = new ObjectConverter
        {
            AssemblyNamePatternToFindTypes = BuildingContext.DefaultAssemblyNamePatternToFindTypes
        };

        return new ObjectMapper(objectConverter);
    }

    /// <summary>
    /// <para>
    /// Sets up driver with auto version detection for the local browser to use.
    /// Gets the name of the local browser to use from <see cref="AtataBuildingContext.LocalBrowserToUseName"/> property.
    /// Then invokes <c>Atata.WebDriverSetup.DriverSetup.AutoSetUpSafely(...)</c> static method
    /// from <c>Atata.WebDriverSetup</c> package.
    /// </para>
    /// <para>
    /// In order to use this method,
    /// ensure that <c>Atata.WebDriverSetup</c> package is installed.
    /// </para>
    /// </summary>
    public void AutoSetUpDriverToUse()
    {
        if (BuildingContext.UsesLocalBrowser)
            InvokeAutoSetUpSafelyMethodOfDriverSetup([BuildingContext.LocalBrowserToUseName]);
    }

    /// <inheritdoc cref="AutoSetUpDriverToUse"/>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task AutoSetUpDriverToUseAsync() =>
        await Task.Run(AutoSetUpDriverToUse);

    /// <summary>
    /// <para>
    /// Sets up drivers with auto version detection for the local configured browsers.
    /// Gets the names of configured local browsers from <see cref="AtataBuildingContext.ConfiguredLocalBrowserNames"/> property.
    /// Then invokes <c>Atata.WebDriverSetup.DriverSetup.AutoSetUpSafely(...)</c> static method
    /// from <c>Atata.WebDriverSetup</c> package.
    /// </para>
    /// <para>
    /// In order to use this method,
    /// ensure that <c>Atata.WebDriverSetup</c> package is installed.
    /// </para>
    /// </summary>
    public void AutoSetUpConfiguredDrivers() =>
        InvokeAutoSetUpSafelyMethodOfDriverSetup(BuildingContext.ConfiguredLocalBrowserNames);

    /// <inheritdoc cref="AutoSetUpConfiguredDrivers"/>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task AutoSetUpConfiguredDriversAsync() =>
        await Task.Run(AutoSetUpConfiguredDrivers);

    private static void InvokeAutoSetUpSafelyMethodOfDriverSetup(IEnumerable<string> browserNames)
    {
        Type driverSetupType = Type.GetType("Atata.WebDriverSetup.DriverSetup,Atata.WebDriverSetup", true);

        var setUpMethod = driverSetupType.GetMethodWithThrowOnError(
            "AutoSetUpSafely",
            BindingFlags.Public | BindingFlags.Static);

        setUpMethod.InvokeStaticAsLambda(browserNames);
    }
}
