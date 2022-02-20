using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Atata
{
    using Should = IObjectVerificationProvider<DirectoryInfo, DirectorySubject>;

    /// <summary>
    /// Provides a set of directory verification extension methods.
    /// </summary>
    public static class DirectoryVerificationProviderExtensions
    {
        /// <summary>
        /// Verifies that directory exists.
        /// </summary>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static DirectorySubject Exist(this Should verifier) =>
            verifier.Owner.Exists.Should.WithSettings(verifier).BeTrue();

        /// <summary>
        /// Verifies that directory contains a file with the specified name.
        /// </summary>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>The owner instance.</returns>
        public static DirectorySubject ContainFile(this Should verifier, string fileName) =>
            verifier.ContainFiles(fileName);

        /// <inheritdoc cref="ContainFiles(Should, IEnumerable{string})"/>
        public static DirectorySubject ContainFiles(this Should should, params string[] fileNames) =>
            should.ContainFiles(fileNames.AsEnumerable());

        /// <summary>
        /// Verifies that directory contains files with the specified names.
        /// </summary>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="fileNames">The file names.</param>
        /// <returns>The owner instance.</returns>
        public static DirectorySubject ContainFiles(this Should verifier, IEnumerable<string> fileNames) =>
            verifier.Owner.Files.Names.Should.WithSettings(verifier).Contain(fileNames);

        /// <summary>
        /// Verifies that directory contains a subdirectory with the specified name.
        /// </summary>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="directoryName">The directory name.</param>
        /// <returns>The owner instance.</returns>
        public static DirectorySubject ContainDirectory(this Should verifier, string directoryName) =>
            verifier.ContainDirectories(directoryName);

        /// <inheritdoc cref="ContainDirectories(Should, IEnumerable{string})"/>
        public static DirectorySubject ContainDirectories(this Should should, params string[] directoryNames) =>
            should.ContainDirectories(directoryNames.AsEnumerable());

        /// <summary>
        /// Verifies that directory contains subdirectories with the specified names.
        /// </summary>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="directoryNames">The directory names.</param>
        /// <returns>The owner instance.</returns>
        public static DirectorySubject ContainDirectories(this Should verifier, IEnumerable<string> directoryNames) =>
            verifier.Owner.Directories.Names.Should.WithSettings(verifier).Contain(directoryNames);
    }
}
