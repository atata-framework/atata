﻿namespace Atata;

/// <summary>
/// Represents the value provider class that wraps enumerable of <see cref="DirectorySubject"/> objects and is hosted in <typeparamref name="TOwner"/> object.
/// </summary>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public class DirectoryEnumerableProvider<TOwner> : EnumerableValueProvider<DirectorySubject, TOwner>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryEnumerableProvider{TOwner}"/> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public DirectoryEnumerableProvider(
        TOwner owner,
        IObjectSource<IEnumerable<DirectorySubject>> objectSource,
        string providerName,
        IAtataExecutionUnit? executionUnit = null)
        : base(owner, objectSource, providerName, executionUnit)
    {
    }

    /// <summary>
    /// Gets the directory names.
    /// </summary>
    public EnumerableValueProvider<ValueProvider<string, DirectorySubject>, TOwner> Names =>
        this.Query(nameof(Names), q => q.Select(x => x.Name));

    /// <summary>
    /// Gets the <see cref="DirectorySubject"/> for the directory with the specified name.
    /// </summary>
    /// <value>
    /// The <see cref="DirectorySubject"/>.
    /// </value>
    /// <param name="directoryName">Name of the directory.</param>
    /// <returns>A <see cref="DirectorySubject"/> instance.</returns>
    public DirectorySubject this[string directoryName]
    {
        get
        {
            var item = Value.First(x => x.Name == directoryName);

            item.ProviderName = $"[\"{directoryName}\"]";

            return item;
        }
    }
}
