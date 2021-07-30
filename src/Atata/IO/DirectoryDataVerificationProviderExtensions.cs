using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Atata
{
    using Should = IDataVerificationProvider<DirectoryInfo, DirectorySubject>;

    /// <summary>
    /// Provides a set of directory verification extension methods.
    /// </summary>
    public static class DirectoryDataVerificationProviderExtensions
    {
        /// <summary>
        /// Verifies that directory exists.
        /// </summary>
        /// <param name="should">The should instance.</param>
        /// <returns>The owner instance.</returns>
        public static DirectorySubject Exist(this Should should) =>
            should.Owner.Exists.Should.WithSettings(should).BeTrue();

        /// <summary>
        /// Verifies that directory contains a file with the specified name.
        /// </summary>
        /// <param name="should">The should instance.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>The owner instance.</returns>
        public static DirectorySubject ContainFile(this Should should, string fileName) =>
            should.ContainFiles(fileName);

        /// <inheritdoc cref="ContainFiles(Should, IEnumerable{string})"/>
        public static DirectorySubject ContainFiles(this Should should, params string[] fileNames) =>
            should.ContainFiles(fileNames.AsEnumerable());

        /// <summary>
        /// Verifies that directory contains files with the specified names.
        /// </summary>
        /// <param name="should">The should instance.</param>
        /// <param name="fileNames">The file names.</param>
        /// <returns>The owner instance.</returns>
        public static DirectorySubject ContainFiles(this Should should, IEnumerable<string> fileNames) =>
            should.Owner.Files.Names.Should.WithSettings(should).Contain(fileNames);

        /// <summary>
        /// Verifies that directory contains a subdirectory with the specified name.
        /// </summary>
        /// <param name="should">The should instance.</param>
        /// <param name="directoryName">The directory name.</param>
        /// <returns>The owner instance.</returns>
        public static DirectorySubject ContainDirectory(this Should should, string directoryName) =>
            should.ContainDirectories(directoryName);

        /// <inheritdoc cref="ContainDirectories(Should, IEnumerable{string})"/>
        public static DirectorySubject ContainDirectories(this Should should, params string[] directoryNames) =>
            should.ContainDirectories(directoryNames.AsEnumerable());

        /// <summary>
        /// Verifies that directory contains subdirectories with the specified names.
        /// </summary>
        /// <param name="should">The should instance.</param>
        /// <param name="directoryNames">The directory names.</param>
        /// <returns>The owner instance.</returns>
        public static DirectorySubject ContainDirectories(this Should should, IEnumerable<string> directoryNames) =>
            should.Owner.Directories.Names.Should.WithSettings(should).Contain(directoryNames);
    }
}
