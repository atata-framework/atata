using System.IO;

namespace Atata
{
    using Should = IDataVerificationProvider<FileInfo, FileSubject>;

    /// <summary>
    /// Provides a set of file verification extension methods.
    /// </summary>
    public static class FileDataVerificationProviderExtensions
    {
        /// <summary>
        /// Verifies that file exists.
        /// </summary>
        /// <param name="should">The should instance.</param>
        /// <returns>The owner instance.</returns>
        public static FileSubject Exist(this Should should) =>
            should.Owner.Exists.Should.WithSettings(should).BeTrue();
    }
}
