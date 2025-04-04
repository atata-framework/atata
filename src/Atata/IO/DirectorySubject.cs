namespace Atata;

using _ = DirectorySubject;

/// <summary>
/// Represents the directory test subject that wraps <see cref="DirectoryInfo"/> object.
/// </summary>
public class DirectorySubject : SubjectBase<DirectoryInfo, _>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="_"/> class.
    /// </summary>
    /// <param name="directoryPath">The directory path.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public DirectorySubject(string directoryPath, string? providerName = null, IAtataExecutionUnit? executionUnit = null)
        : this(
            DynamicObjectSource.Create(() => new DirectoryInfo(directoryPath)),
            providerName ?? BuildProviderName(directoryPath),
            executionUnit) =>
        directoryPath.CheckNotNullOrEmpty(nameof(directoryPath));

    /// <summary>
    /// Initializes a new instance of the <see cref="_"/> class.
    /// </summary>
    /// <param name="directoryInfo">The <see cref="DirectoryInfo"/> object.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public DirectorySubject(DirectoryInfo directoryInfo, string? providerName = null, IAtataExecutionUnit? executionUnit = null)
        : this(
            new StaticObjectSource<DirectoryInfo>(directoryInfo.CheckNotNull(nameof(directoryInfo))),
            providerName ?? BuildProviderName(directoryInfo.FullName),
            executionUnit)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="_"/> class.
    /// </summary>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public DirectorySubject(IObjectSource<DirectoryInfo> objectSource, string providerName, IAtataExecutionUnit? executionUnit = null)
        : base(objectSource, providerName, executionUnit)
    {
    }

    /// <summary>
    /// Gets a value provider indicating whether the directory exists.
    /// </summary>
    public ValueProvider<bool, _> Exists =>
        this.DynamicValueOf(x => x.Exists);

    /// <summary>
    /// Gets a value provider of the directory name.
    /// </summary>
    public ValueProvider<string, _> Name =>
        this.ValueOf(x => x.Name);

    /// <summary>
    /// Gets a value provider of the directory full name (path).
    /// </summary>
    public ValueProvider<string, _> FullName =>
        this.ValueOf(x => x.FullName);

    /// <summary>
    /// Gets the subdirectories of the current directory.
    /// </summary>
    public SubdirectoriesProvider Directories =>
        new(this, nameof(Directories), ExecutionUnit);

    /// <summary>
    /// Gets the files of the current directory.
    /// </summary>
    public DirectoryFilesProvider Files =>
        new(this, nameof(Files), ExecutionUnit);

    private static string BuildProviderName(string directoryPath) =>
        $"\"{directoryPath}\" directory";
}
