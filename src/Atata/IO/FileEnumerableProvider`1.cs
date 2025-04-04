#nullable enable

namespace Atata;

/// <summary>
/// Represents the value provider class that wraps enumerable of <see cref="FileSubject"/> objects and is hosted in <typeparamref name="TOwner"/> object.
/// </summary>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public class FileEnumerableProvider<TOwner> : EnumerableValueProvider<FileSubject, TOwner>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileEnumerableProvider{TOwner}"/> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public FileEnumerableProvider(
        TOwner owner,
        IObjectSource<IEnumerable<FileSubject>> objectSource,
        string providerName,
        IAtataExecutionUnit? executionUnit = null)
        : base(owner, objectSource, providerName, executionUnit)
    {
    }

    /// <summary>
    /// Gets the file names.
    /// </summary>
    public EnumerableValueProvider<ValueProvider<string, FileSubject>, TOwner> Names =>
        this.Query(nameof(Names), q => q.Select(x => x.Name));

    /// <summary>
    /// Gets the <see cref="FileSubject"/> for the file with the specified name.
    /// </summary>
    /// <value>The <see cref="FileSubject"/>.</value>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>A <see cref="FileSubject"/> instance.</returns>
    public FileSubject this[string fileName]
    {
        get
        {
            var item = Value.First(x => x.Name == fileName);

            item.ProviderName = $"[\"{fileName}\"]";

            return item;
        }
    }
}
