namespace Atata.MSTest;

internal static class TestSuiteTypeResolver
{
    private static readonly object s_assemblyResolveLock = new();

    private static readonly ConcurrentDictionary<string, Type> s_typesByName = [];

    internal static Assembly? Assembly { get; set; }

    internal static Type Resolve(string typeName)
    {
        if (Assembly is null)
        {
            lock (s_assemblyResolveLock)
            {
                if (Assembly is null)
                {
                    Assembly[] assemblies = AssemblyFinder.FindAllByPattern(AtataContext.GlobalProperties.AssemblyNamePatternToFindTypes);
                    Type type = TypeFinder.FindInAssemblies(typeName, assemblies);

                    Assembly = type.Assembly;

                    return type;
                }
            }
        }

        return s_typesByName.GetOrAdd(typeName, DoResolve);
    }

    internal static Type DoResolve(string typeName) =>
        Assembly!.GetType(typeName, true);
}
