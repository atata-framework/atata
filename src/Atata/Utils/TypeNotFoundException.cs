namespace Atata;

public class TypeNotFoundException : Exception
{
    public TypeNotFoundException()
    {
    }

    public TypeNotFoundException(string? message)
        : base(message)
    {
    }

    public TypeNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public static TypeNotFoundException For(string typeName, IEnumerable<Assembly> assembliesToFindIn) =>
        new($"Failed to find \"{typeName}\" type. Tried to find in assemblies: {string.Join(", ", assembliesToFindIn.Select(x => x.GetName().Name))}.");
}
