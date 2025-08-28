namespace Atata;

/// <summary>
/// Contains global properties that should be configured as early as possible,
/// typically in global setup method
/// before any creation of <see cref="AtataContext"/>, and not changed later,
/// because these properties should have the same values for all the contexts within an execution.
/// </summary>
public sealed class AtataContextGlobalProperties
{
    [Obsolete("Use DefaultArtifactsRootPathTemplateExcludingRunStart instead.")] // Obsolete since v4.0.0.
    public const string DefaultArtifactsRootPathTemplateWithoutBuildStartFolder =
        "{basedir}/artifacts";

    public const string DefaultArtifactsRootPathTemplateExcludingRunStart =
        "{basedir}/artifacts";

    public const string DefaultArtifactsRootPathTemplate =
        DefaultArtifactsRootPathTemplateExcludingRunStart + "/{run-start:yyyyMMddTHHmmss}";

    private AtataContextModeOfCurrent _modeOfCurrent = AtataContextModeOfCurrent.AsyncLocal;

    private TimeZoneInfo _timeZone = TimeZoneInfo.Local;

    private string _artifactsRootPathTemplate = DefaultArtifactsRootPathTemplate;

    private Lazy<DirectorySubject> _lazyArtifactsRoot;

    private string _assemblyNamePatternToFindTypes = @"^(?!System($|\..+)|mscorlib$|netstandard$|Microsoft\..+)";

    internal AtataContextGlobalProperties()
    {
        InitializeRunStart();
        _lazyArtifactsRoot = new(CreateArtifactsRootDirectorySubject);
        SetObjectCreationalProperties();
    }

    /// <summary>
    /// Gets or sets the mode of <see cref="AtataContext.Current"/> property.
    /// The default value is <see cref="AtataContextModeOfCurrent.AsyncLocal"/>.
    /// </summary>
    public AtataContextModeOfCurrent ModeOfCurrent
    {
        get => _modeOfCurrent;
        set
        {
            _modeOfCurrent = value;

            RetrySettings.ThreadBoundary = value switch
            {
                AtataContextModeOfCurrent.AsyncLocal => RetrySettingsThreadBoundary.AsyncLocal,
                AtataContextModeOfCurrent.ThreadStatic => RetrySettingsThreadBoundary.ThreadStatic,
                _ => RetrySettingsThreadBoundary.Static
            };
        }
    }

    [Obsolete("Use RunStartUtc instead.")] // Obsolete since v4.0.0.
    public DateTime BuildStartUtc =>
        RunStartUtc;

    /// <summary>
    /// Gets the run start UTC date/time.
    /// Has the same value for all the tests being executed within one run.
    /// </summary>
    public DateTime RunStartUtc { get; private set; } = DateTime.UtcNow;

    [Obsolete("Use RunStart instead.")] // Obsolete since v4.0.0.
    public DateTime BuildStart =>
        RunStart;

    /// <summary>
    /// Gets the run start date/time in <see cref="TimeZone"/> (local by default).
    /// Has the same value for all the tests being executed within one run.
    /// </summary>
    public DateTime RunStart { get; private set; }

    /// <summary>
    /// Gets or sets the root namespace.
    /// That namespace can be used during the process of Artifacts directory path creation,
    /// basically the root namespace is cut off for Artifacts path.
    /// The default value is <see langword="null"/>.
    /// </summary>
    public string? RootNamespace { get; set; }

    /// <summary>
    /// Gets or sets the time zone.
    /// The default value is <see cref="TimeZoneInfo.Local"/>.
    /// </summary>
    public TimeZoneInfo TimeZone
    {
        get => _timeZone;
        set
        {
            Guard.ThrowIfNull(value);
            _timeZone = value;
            InitializeRunStart();
        }
    }

    /// <summary>
    /// <para>
    /// Gets or sets the path template of the Artifacts Root directory.
    /// The default value is <c>"{basedir}/artifacts/{run-start:yyyyMMddTHHmmss}"</c>.
    /// </para>
    /// <para>
    /// The list of supported variables:
    /// </para>
    /// <list type="bullet">
    /// <item><c>{basedir}</c></item>
    /// <item><c>{run-start}</c></item>
    /// <item><c>{run-start-utc}</c></item>
    /// </list>
    /// </summary>
    public string ArtifactsRootPathTemplate
    {
        get => _artifactsRootPathTemplate;
        set
        {
            _artifactsRootPathTemplate = value;

            if (_lazyArtifactsRoot.IsValueCreated)
                _lazyArtifactsRoot = new(CreateArtifactsRootDirectorySubject);
        }
    }

    /// <summary>
    /// Gets the <see cref="DirectorySubject"/> of Artifacts Root directory.
    /// <see cref="ArtifactsRootPathTemplate"/> property is used to configure the Artifacts Root path.
    /// </summary>
    public DirectorySubject ArtifactsRoot => _lazyArtifactsRoot.Value;

    /// <summary>
    /// Gets the path of Artifacts Root directory.
    /// <see cref="ArtifactsRootPathTemplate"/> property is used to configure the Artifacts Root path.
    /// </summary>
    public string ArtifactsRootPath => ArtifactsRoot.FullName.Value;

    /// <summary>
    /// Gets or sets the artifacts path factory.
    /// The default value is a default instance of <see cref="TestInfoBasedHierarchicalArtifactsPathFactory"/>.
    /// Alternatively, <see cref="TestInfoBasedHierarchicalArtifactsPathFactory"/> can be set with a sub-folder name for "suite" contexts,
    /// by passing a string argument into constructor.
    /// Another built-in factories are:
    /// <see cref="ContextIdBasedHierarchicalArtifactsPathFactory"/> and
    /// <see cref="ContextIdBasedArtifactsPathFactory"/>.
    /// A custom factory can also be set to follow a custom structure of artifacts directories.
    /// </summary>
    public IArtifactsPathFactory ArtifactsPathFactory { get; set; } =
        new TestInfoBasedHierarchicalArtifactsPathFactory();

    /// <summary>
    /// Gets or sets the assembly name pattern that is used to filter assemblies to find types in them
    /// such as events, event handlers, attributes, components, etc.
    /// The default value is <c>@"^(?!System($|\..+$)|mscorlib$|netstandard$|Microsoft\..+)"</c>,
    /// which filters non-system assemblies.
    /// </summary>
    public string AssemblyNamePatternToFindTypes
    {
        get => _assemblyNamePatternToFindTypes;
        set
        {
            _assemblyNamePatternToFindTypes = value;
            SetObjectCreationalProperties();
        }
    }

    /// <summary>
    /// Gets the object converter.
    /// </summary>
    public IObjectConverter ObjectConverter { get; private set; } = null!;

    /// <summary>
    /// Gets the object mapper.
    /// </summary>
    public IObjectMapper ObjectMapper { get; private set; } = null!;

    /// <summary>
    /// Gets the object creator.
    /// </summary>
    public IObjectCreator ObjectCreator { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the identifier generator.
    /// The default value is an instance of <see cref="Alphanumeric4AtataIdGenerator"/>.
    /// </summary>
    public IAtataIdGenerator IdGenerator { get; set; } = new Alphanumeric4AtataIdGenerator();

    [Obsolete("Use UseDefaultArtifactsRootPathTemplateIncludingRunStart instead.")] // Obsolete since v4.0.0.
    public AtataContextGlobalProperties UseDefaultArtifactsRootPathTemplateIncludingBuildStart(bool include) =>
        UseDefaultArtifactsRootPathTemplateIncludingRunStart(include);

    /// <summary>
    /// Sets the default Artifacts Root path template with optionally
    /// including <c>"{run-start:yyyyMMddTHHmmss}"</c> folder in the path.
    /// </summary>
    /// <param name="include">
    /// Whether to include the <c>"{run-start:yyyyMMddTHHmmss}"</c> folder in the path.
    /// The default value is <see langword="true"/>.
    /// </param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseDefaultArtifactsRootPathTemplateIncludingRunStart(bool include = true) =>
        UseArtifactsRootPathTemplate(include
            ? DefaultArtifactsRootPathTemplate
            : DefaultArtifactsRootPathTemplateExcludingRunStart);

    /// <summary>
    /// <para>
    /// Sets the path template of the Artifacts Root directory.
    /// The default value is <c>"{basedir}/artifacts/{run-start:yyyyMMddTHHmmss}"</c>.
    /// </para>
    /// <para>
    /// The list of supported variables:
    /// <list type="bullet">
    /// <item><c>{basedir}</c></item>
    /// <item><c>{run-start}</c></item>
    /// <item><c>{run-start-utc}</c></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="directoryPathTemplate">The directory path template.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseArtifactsRootPathTemplate(string directoryPathTemplate)
    {
        Guard.ThrowIfNullOrWhitespace(directoryPathTemplate);

        ArtifactsRootPathTemplate = directoryPathTemplate;
        return this;
    }

    /// <summary>
    /// Sets the artifacts path factory.
    /// </summary>
    /// <param name="artifactsPathFactory">The artifacts path factory.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseArtifactsPathFactory(IArtifactsPathFactory artifactsPathFactory)
    {
        Guard.ThrowIfNull(artifactsPathFactory);

        ArtifactsPathFactory = artifactsPathFactory;
        return this;
    }

    /// <summary>
    /// Sets the root namespace with the namespace of the specified <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">The type from which namespace should be taken.</typeparam>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseRootNamespaceOf<T>() =>
        UseRootNamespaceOf(typeof(T));

    /// <summary>
    /// Sets the root namespace with the namespace of the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type from which namespace should be taken.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseRootNamespaceOf(Type type)
    {
        RootNamespace = type.Namespace;
        return this;
    }

    /// <summary>
    /// Sets the root namespace.
    /// </summary>
    /// <param name="rootNamespace">The root namespace.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseRootNamespace(string? rootNamespace)
    {
        RootNamespace = rootNamespace;
        return this;
    }

    /// <summary>
    /// Sets the UTC time zone.
    /// </summary>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseUtcTimeZone() =>
        UseTimeZone(TimeZoneInfo.Utc);

    /// <summary>
    /// Sets the time zone by identifier, which corresponds to the <see cref="TimeZoneInfo.Id"/> property.
    /// </summary>
    /// <param name="timeZoneId">The time zone identifier.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseTimeZone(string timeZoneId)
    {
        Guard.ThrowIfNullOrWhitespace(timeZoneId);
        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        return UseTimeZone(timeZone);
    }

    /// <summary>
    /// Sets the time zone.
    /// </summary>
    /// <param name="timeZone">The time zone.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseTimeZone(TimeZoneInfo timeZone)
    {
        Guard.ThrowIfNull(timeZone);

        TimeZone = timeZone;
        return this;
    }

    /// <summary>
    /// Sets the mode of <see cref="AtataContext.Current"/> property.
    /// The default value is <see cref="AtataContextModeOfCurrent.AsyncLocal"/>.
    /// </summary>
    /// <param name="mode">The mode.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseModeOfCurrent(AtataContextModeOfCurrent mode)
    {
        ModeOfCurrent = mode;
        return this;
    }

    /// <summary>
    /// Sets the assembly name regex pattern that is used to filter assemblies to find types in them,
    /// such as events, event handlers, attributes, components, etc.
    /// The default value is <c>@"^(?!System($|\..+$)|mscorlib$|netstandard$|Microsoft\..+)"</c>,
    /// which filters non-system assemblies.
    /// </summary>
    /// <param name="pattern">The assembly name regex pattern.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseAssemblyNamePatternToFindTypes(string pattern)
    {
        Guard.ThrowIfNullOrWhitespace(pattern);

        AssemblyNamePatternToFindTypes = pattern;
        return this;
    }

    /// <summary>
    /// Sets the identifier generator.
    /// The default value is an instance of <see cref="Alphanumeric4AtataIdGenerator"/>.
    /// </summary>
    /// <param name="idGenerator">The identifier generator.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseIdGenerator(IAtataIdGenerator idGenerator)
    {
        Guard.ThrowIfNull(idGenerator);

        IdGenerator = idGenerator;
        return this;
    }

    internal DateTime ConvertToTimeZone(DateTime utcDateTime) =>
        TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, _timeZone);

    private void InitializeRunStart() =>
        RunStart = ConvertToTimeZone(RunStartUtc);

    private DirectorySubject CreateArtifactsRootDirectorySubject()
    {
        Dictionary<string, object?> variables = new()
        {
            ["basedir"] = AppDomain.CurrentDomain.BaseDirectory,
            ["run-start"] = RunStart,
            ["run-start-utc"] = RunStartUtc
        };

        string path = TemplateStringTransformer.Transform(_artifactsRootPathTemplate, variables);

        return new(path, nameof(ArtifactsRoot));
    }

    private void SetObjectCreationalProperties()
    {
        ObjectConverter = new ObjectConverter
        {
            AssemblyNamePatternToFindTypes = _assemblyNamePatternToFindTypes
        };
        ObjectMapper = new ObjectMapper(ObjectConverter);
        ObjectCreator = new ObjectCreator(ObjectConverter, ObjectMapper);
    }
}
