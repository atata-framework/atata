namespace Atata;

[Serializable]
public class AssemblyNotFoundException : Exception
{
    public AssemblyNotFoundException()
    {
    }

    public AssemblyNotFoundException(string message)
        : base(message)
    {
    }

    public AssemblyNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected AssemblyNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public static AssemblyNotFoundException For(string assemblyName) =>
        new($"Failed to find \"{assemblyName}\" assembly.");
}
