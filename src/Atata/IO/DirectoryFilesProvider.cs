using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the provider of enumerable <see cref="FileSubject"/> objects that represent the files in a certain directory.
    /// </summary>
    public class DirectoryFilesProvider : EnumerableProvider<FileSubject, DirectorySubject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryFilesProvider"/> class.
        /// </summary>
        /// <param name="owner">The owner, which is the parent directory subject.</param>
        /// <param name="providerName">Name of the provider.</param>
        public DirectoryFilesProvider(DirectorySubject owner, string providerName)
            : base(
                owner,
                new DynamicObjectSource<IEnumerable<FileSubject>, DirectoryInfo>(
                    owner,
                    x => x.EnumerateFiles().Select((file, i) => new FileSubject(file, $"[{i}]"))),
                providerName)
        {
        }

        /// <summary>
        /// Gets the file names.
        /// </summary>
        public EnumerableProvider<ValueProvider<string, FileSubject>, DirectorySubject> Names =>
            this.Query(nameof(Names), q => q.Select(x => x.Name));

        /// <summary>
        /// Gets the <see cref="FileSubject"/> for the file with the specified name.
        /// </summary>
        /// <value>The <see cref="FileSubject"/>.</value>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A <see cref="FileSubject"/> instance.</returns>
        public FileSubject this[string fileName] =>
            new FileSubject(
                Path.Combine(Owner.Object.FullName, fileName),
                $"[\"{fileName}\"]")
            {
                SourceProviderName = ProviderName
            };
    }
}
