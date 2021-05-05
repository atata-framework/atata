using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Atata
{
    using _ = DirectorySubject;

    /// <summary>
    /// Represents the directory test subject that wraps <see cref="DirectoryInfo"/> object.
    /// </summary>
    public class DirectorySubject : SubjectBase<DirectoryInfo, _>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectorySubject"/> class.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="providerName">Name of the provider.</param>
        public DirectorySubject(string directoryPath, string providerName = null)
            : this(new DirectoryInfo(directoryPath), providerName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectorySubject"/> class.
        /// </summary>
        /// <param name="directoryInfo">The <see cref="DirectoryInfo"/> object.</param>
        /// <param name="providerName">Name of the provider.</param>
        public DirectorySubject(DirectoryInfo directoryInfo, string providerName = null)
            : this(
                new StaticObjectSource<DirectoryInfo>(directoryInfo.CheckNotNull(nameof(directoryInfo))),
                providerName ?? BuildProviderName(directoryInfo))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectorySubject"/> class.
        /// </summary>
        /// <param name="objectSource">The object source.</param>
        /// <param name="providerName">Name of the provider.</param>
        public DirectorySubject(IObjectSource<DirectoryInfo> objectSource, string providerName)
            : base(objectSource, providerName)
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
        /// Gets the directories of the current directory.
        /// </summary>
        public DirectoryEnumerableProvider<_> Directories =>
            new DirectoryEnumerableProvider<_>(
                Owner,
                new DynamicObjectSource<IEnumerable<_>, DirectoryInfo>(
                    this,
                    x => x.EnumerateDirectories().Select((dir, i) => new _(dir, $"[{i}]"))),
                nameof(Directories));

        /// <summary>
        /// Gets the files of the current directory.
        /// </summary>
        public FileEnumerableProvider<_> Files =>
            new FileEnumerableProvider<_>(
                Owner,
                new DynamicObjectSource<IEnumerable<FileSubject>, DirectoryInfo>(
                    this,
                    x => x.EnumerateFiles().Select((file, i) => new FileSubject(file, $"[{i}]"))),
                nameof(Files));

        private static string BuildProviderName(DirectoryInfo directoryInfo) =>
            $"\"{directoryInfo.FullName}\" directory";
    }
}
