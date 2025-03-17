#nullable enable

namespace Atata;

public static class MethodFinder
{
    public static IEnumerable<(MethodInfo Method, TAttribute Attribute)> FindAllWithAttribute<TAttribute>(Type type, BindingFlags bindingFlags)
        where TAttribute : notnull, Attribute
    {
        MethodInfo[] methods = type.GetMethods(bindingFlags);

        foreach (MethodInfo method in methods)
        {
            if (method.TryGetCustomAttribute(out TAttribute? attribute))
                yield return (method, attribute);
        }
    }
}
