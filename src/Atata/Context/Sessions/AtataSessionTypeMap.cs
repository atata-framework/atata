#nullable enable

namespace Atata;

internal static class AtataSessionTypeMap
{
    private static readonly ConcurrentDictionary<Type, Type?> s_builderSessionTypeMap = [];

    internal static string ResolveSessionTypedName(IAtataSessionBuilder builder)
    {
        Type sessionType = ResolveSessionTypeByBuilderType(builder.GetType());
        string? sessionName = builder.Name;

        return AtataSession.BuildTypedName(sessionType, sessionName);
    }

    internal static Type ResolveSessionTypeByBuilderType(Type builderType)
    {
        Type? sessionType = s_builderSessionTypeMap.GetOrAdd(builderType, DoResolveSessionTypeByBuilderType);

        return sessionType
            ?? throw new TypeNotFoundException($"{builderType} type doesn't inherit {typeof(AtataSessionBuilder<,>)} type.");
    }

    private static Type? DoResolveSessionTypeByBuilderType(Type builderType)
    {
        Type? concreteBuilderType = builderType.GetBaseTypeOfRawGeneric(typeof(AtataSessionBuilder<,>));

        return concreteBuilderType?.GetGenericArguments()[0];
    }
}
