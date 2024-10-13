namespace Atata;

/// <summary>
/// Contains global properties that should be configured as early as possible,
/// typically in global setup method
/// before any creation of <see cref="AtataContext"/>, and not changed later,
/// because these properties should have the same values for all the contexts within an execution.
/// </summary>
public sealed class AtataContextGlobalProperties
{
    public const string DefaultArtifactsRootPathTemplateWithoutBuildStartFolder =
        "{basedir}/artifacts";

    public const string DefaultArtifactsRootPathTemplate =
        DefaultArtifactsRootPathTemplateWithoutBuildStartFolder + "/{build-start:yyyyMMddTHHmmss}";

    private AtataContextModeOfCurrent _modeOfCurrent = AtataContextModeOfCurrent.AsyncLocal;

    private TimeZoneInfo _timeZone = TimeZoneInfo.Local;

    private string _artifactsRootPathTemplate = DefaultArtifactsRootPathTemplate;

    private Lazy<DirectorySubject> _lazyArtifactsRoot;

    private string _assemblyNamePatternToFindTypes = @"^(?!System($|\..+)|mscorlib$|netstandard$|Microsoft\..+)";

    internal AtataContextGlobalProperties()
    {
        InitBuildStart();
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

    /// <summary>
    /// Gets the build start UTC date/time.
    /// Has the same value for all the tests being executed within one build.
    /// </summary>
    public DateTime BuildStartUtc { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the build start date/time in <see cref="TimeZone"/> (local by default).
    /// Has the same value for all the tests being executed within one build.
    /// </summary>
    public DateTime BuildStart { get; private set; }

    /// <summary>
    /// Gets or sets the time zone.
    /// The default value is <see cref="TimeZoneInfo.Local"/>.
    /// </summary>
    public TimeZoneInfo TimeZone
    {
        get => _timeZone;
        set
        {
            _timeZone = value.CheckNotNull(nameof(value));
            InitBuildStart();
        }
    }

    /// <summary>
    /// <para>
    /// Gets or sets the path template of the Artifacts Root directory.
    /// The default value is <c>"{basedir}/artifacts/{build-start:yyyyMMddTHHmmss}"</c>.
    /// </para>
    /// <para>
    /// The list of supported variables:
    /// </para>
    /// <list type="bullet">
    /// <item><c>{basedir}</c></item>
    /// <item><c>{build-start}</c></item>
    /// <item><c>{build-start-utc}</c></item>
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
    public IObjectConverter ObjectConverter { get; private set; }

    /// <summary>
    /// Gets the object mapper.
    /// </summary>
    public IObjectMapper ObjectMapper { get; private set; }

    /// <summary>
    /// Gets the object creator.
    /// </summary>
    public IObjectCreator ObjectCreator { get; private set; }

    /// <summary>
    /// Gets or sets the identifier generator.
    /// The default value is an instance of <see cref="Alphanumeric4AtataIdGenerator"/>.
    /// </summary>
    public IAtataIdGenerator IdGenerator { get; set; } = new Alphanumeric4AtataIdGenerator();

    /// <summary>
    /// Sets the default Artifacts Root path template with optionally
    /// including <c>"{build-start:yyyyMMddTHHmmss}"</c> folder in the path.
    /// </summary>
    /// <param name="include">Whether to include the <c>"{build-start:yyyyMMddTHHmmss}"</c> folder in the path.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseDefaultArtifactsRootPathTemplateIncludingBuildStart(bool include) =>
        UseArtifactsRootPathTemplate(include
            ? DefaultArtifactsRootPathTemplate
            : DefaultArtifactsRootPathTemplateWithoutBuildStartFolder);

    /// <summary>
    /// <para>
    /// Sets the path template of the Artifacts Root directory.
    /// The default value is <c>"{basedir}/artifacts/{build-start:yyyyMMddTHHmmss}"</c>.
    /// </para>
    /// <para>
    /// The list of supported variables:
    /// </para>
    /// <list type="bullet">
    /// <item><c>{basedir}</c></item>
    /// <item><c>{build-start}</c></item>
    /// <item><c>{build-start-utc}</c></item>
    /// </list>
    /// </summary>
    /// <param name="directoryPathTemplate">The directory path template.</param>
    /// <returns>The same <see cref="AtataContextGlobalProperties"/> instance.</returns>
    public AtataContextGlobalProperties UseArtifactsRootPathTemplate(string directoryPathTemplate)
    {
        directoryPathTemplate.CheckNotNullOrWhitespace(nameof(directoryPathTemplate));

        ArtifactsRootPathTemplate = directoryPathTemplate;
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
        timeZoneId.CheckNotNullOrWhitespace(nameof(timeZoneId));
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
        timeZone.CheckNotNull(nameof(timeZone));

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
        pattern.CheckNotNullOrWhitespace(nameof(pattern));

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
        idGenerator.CheckNotNull(nameof(idGenerator));

        IdGenerator = idGenerator;
        return this;
    }

    private void InitBuildStart() =>
        BuildStart = TimeZoneInfo.ConvertTimeFromUtc(BuildStartUtc, TimeZone);

    private DirectorySubject CreateArtifactsRootDirectorySubject()
    {
        Dictionary<string, object> variables = new()
        {
            ["basedir"] = AppDomain.CurrentDomain.BaseDirectory,
            ["build-start"] = BuildStart,
            ["build-start-utc"] = BuildStartUtc
        };

        string path = TemplateStringTransformer.Transform(_artifactsRootPathTemplate, variables);

        return new DirectorySubject(path, nameof(ArtifactsRoot));
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
