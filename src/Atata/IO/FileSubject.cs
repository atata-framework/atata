namespace Atata;

using _ = FileSubject;

/// <summary>
/// Represents the file test subject that wraps <see cref="FileInfo"/> object.
/// </summary>
public class FileSubject : SubjectBase<FileInfo, _>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="_"/> class.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public FileSubject(string filePath, string providerName = null, IAtataExecutionUnit executionUnit = null)
        : this(
            DynamicObjectSource.Create(() => new FileInfo(filePath)),
            providerName ?? BuildProviderName(filePath),
            executionUnit) =>
        filePath.CheckNotNullOrEmpty(nameof(filePath));

    /// <summary>
    /// Initializes a new instance of the <see cref="_"/> class.
    /// </summary>
    /// <param name="fileInfo">The <see cref="FileInfo"/> object.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public FileSubject(FileInfo fileInfo, string providerName = null, IAtataExecutionUnit executionUnit = null)
        : this(
            new StaticObjectSource<FileInfo>(fileInfo.CheckNotNull(nameof(fileInfo))),
            providerName ?? BuildProviderName(fileInfo.FullName),
            executionUnit)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="_"/> class.
    /// </summary>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public FileSubject(IObjectSource<FileInfo> objectSource, string providerName, IAtataExecutionUnit executionUnit = null)
        : base(objectSource, providerName, executionUnit)
    {
    }

    /// <summary>
    /// Gets a value provider indicating whether the file exists.
    /// </summary>
    public ValueProvider<bool, _> Exists =>
        this.DynamicValueOf(x => x.Exists);

    /// <summary>
    /// Gets a value provider of the file name.
    /// </summary>
    public ValueProvider<string, _> Name =>
        this.ValueOf(x => x.Name);

    /// <summary>
    /// Gets a value provider of the full file name.
    /// </summary>
    public ValueProvider<string, _> FullName =>
        this.ValueOf(x => x.FullName);

    /// <summary>
    /// Gets a value provider of the file extension, like <c>.txt</c>.
    /// </summary>
    public ValueProvider<string, _> Extension =>
        this.ValueOf(x => x.Extension);

    /// <summary>
    /// Gets a value provider of the file name without extension.
    /// </summary>
    public ValueProvider<string, _> NameWithoutExtension =>
        this.ValueOf(x => Path.GetFileNameWithoutExtension(x.Name), nameof(NameWithoutExtension));

    /// <summary>
    /// Gets a value provider that determines if the file is read only.
    /// </summary>
    public ValueProvider<bool, _> IsReadOnly =>
        this.DynamicValueOf(x => x.IsReadOnly);

    /// <summary>
    /// Gets a value provider of the size of the file in bytes.
    /// </summary>
    public ValueProvider<long, _> Length =>
        this.DynamicValueOf(x => x.Length);

    /// <summary>
    /// Returns a new <see cref="Subject{TObject}"/> for the file text.
    /// </summary>
    /// <returns>A new <see cref="Subject{TObject}"/>.</returns>
    public Subject<string> ReadAllText() =>
        ResultOf(_ => File.ReadAllText(FullName), $"{nameof(ReadAllText)}()");

    private static string BuildProviderName(string filePath) =>
        $"\"{filePath}\" file";
}
