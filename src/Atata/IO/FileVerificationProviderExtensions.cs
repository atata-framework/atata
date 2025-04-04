#nullable enable

namespace Atata;

using Should = IObjectVerificationProvider<FileInfo, FileSubject>;

/// <summary>
/// Provides a set of file verification extension methods.
/// </summary>
public static class FileVerificationProviderExtensions
{
    /// <summary>
    /// Verifies that file exists.
    /// </summary>
    /// <param name="verifier">The should instance.</param>
    /// <returns>The owner instance.</returns>
    public static FileSubject Exist(this Should verifier) =>
        verifier.Owner.Exists.Should.WithSettings(verifier).BeTrue();
}
