using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the provider of enumerable <see cref="FileSubject"/> objects that represent the files in a certain directory.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    // TODO: In v2 inherit from EnumerableProvider<FileSubject, TOwner>.
    public class DirectoryFilesProvider<TOwner> : FileEnumerableProvider<TOwner>
    {
        private readonly DirectorySubject parentDirectorySubject;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryFilesProvider{TOwner}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="parentDirectorySubject">The parent directory subject.</param>
        /// <param name="providerName">Name of the provider.</param>
        public DirectoryFilesProvider(
            TOwner owner,
            DirectorySubject parentDirectorySubject,
            string providerName)
            : base(
                owner,
                new DynamicObjectSource<IEnumerable<FileSubject>, DirectoryInfo>(
                    parentDirectorySubject,
                    x => x.EnumerateFiles().Select((file, i) => new FileSubject(file, $"[{i}]"))),
                providerName)
        {
            this.parentDirectorySubject = parentDirectorySubject;
        }

        /// <inheritdoc/>
        public override FileSubject this[string fileName] =>
            new FileSubject(
                new FileInfo(Path.Combine(parentDirectorySubject.Value.FullName, fileName)),
                $"[\"{fileName}\"]")
            {
                SourceProviderName = ProviderName
            };
    }
}
