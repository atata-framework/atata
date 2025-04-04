#nullable enable

namespace Atata;

/// <summary>
/// Represents the provider of enumerable <see cref="DirectorySubject"/> objects that represent the subdirectories of a certain directory.
/// </summary>
public class SubdirectoriesProvider : EnumerableValueProvider<DirectorySubject, DirectorySubject>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubdirectoriesProvider"/> class.
    /// </summary>
    /// <param name="owner">The owner, which is the parent directory subject.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public SubdirectoriesProvider(
        DirectorySubject owner,
        string providerName,
        IAtataExecutionUnit? executionUnit = null)
        : base(
            owner,
            new DynamicObjectSource<IEnumerable<DirectorySubject>, DirectoryInfo>(
                owner,
                x => x.EnumerateDirectories().Select((dir, i) => new DirectorySubject(dir, $"[{i}]", executionUnit))),
            providerName,
            executionUnit)
    {
    }

    /// <summary>
    /// Gets the directory names.
    /// </summary>
    public EnumerableValueProvider<ValueProvider<string, DirectorySubject>, DirectorySubject> Names =>
        this.Query(nameof(Names), q => q.Select(x => x.Name));

    /// <summary>
    /// Gets the <see cref="DirectorySubject"/> for the directory with the specified name.
    /// </summary>
    /// <value>
    /// The <see cref="DirectorySubject"/>.
    /// </value>
    /// <param name="directoryName">Name of the directory.</param>
    /// <returns>A <see cref="DirectorySubject"/> instance.</returns>
    public DirectorySubject this[string directoryName] =>
        new(
            Path.Combine(Owner.Object.FullName, directoryName),
            $"[\"{directoryName}\"]",
            ExecutionUnit)
        {
            SourceProviderName = ProviderName
        };
}
