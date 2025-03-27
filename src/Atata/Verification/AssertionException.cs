#nullable enable

namespace Atata;

/// <summary>
/// Represents an assertion failure error.
/// </summary>
/// <seealso cref="System.Exception" />
public class AssertionException : Exception
{
    public AssertionException()
    {
    }

    public AssertionException(string? message)
        : base(message)
    {
    }

    public AssertionException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
