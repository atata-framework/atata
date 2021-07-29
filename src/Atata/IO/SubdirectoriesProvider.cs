using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the provider of enumerable <see cref="DirectorySubject"/> objects that represent the subdirectories of a certain directory.
    /// </summary>
    // TODO: In v2 inherit from EnumerableProvider<DirectorySubject, TOwner>.
    public class SubdirectoriesProvider : DirectoryEnumerableProvider<DirectorySubject>
    {
        private readonly DirectorySubject parentDirectorySubject;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubdirectoriesProvider"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="parentDirectorySubject">The parent directory subject.</param>
        /// <param name="providerName">Name of the provider.</param>
        public SubdirectoriesProvider(
            DirectorySubject owner,
            DirectorySubject parentDirectorySubject,
            string providerName)
            : base(
                owner,
                new DynamicObjectSource<IEnumerable<DirectorySubject>, DirectoryInfo>(
                    parentDirectorySubject,
                    x => x.EnumerateDirectories().Select((dir, i) => new DirectorySubject(dir, $"[{i}]"))),
                providerName)
        {
            this.parentDirectorySubject = parentDirectorySubject;
        }

        /// <inheritdoc/>
        public override DirectorySubject this[string directoryName] =>
            new DirectorySubject(
                new DirectoryInfo(Path.Combine(parentDirectorySubject.Value.FullName, directoryName)),
                $"[\"{directoryName}\"]")
            {
                SourceProviderName = ProviderName
            };
    }
}
