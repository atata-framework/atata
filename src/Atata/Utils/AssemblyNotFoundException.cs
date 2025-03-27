#nullable enable

namespace Atata;

public class AssemblyNotFoundException : Exception
{
    public AssemblyNotFoundException()
    {
    }

    public AssemblyNotFoundException(string? message)
        : base(message)
    {
    }

    public AssemblyNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public static AssemblyNotFoundException For(string assemblyName) =>
        new($"Failed to find \"{assemblyName}\" assembly.");
}
