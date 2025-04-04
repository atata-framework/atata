namespace Atata;

public static class AtataContextScopesExtensions
{
    public static bool Contains(this AtataContextScopes scopes, AtataContextScope scope) =>
        scope switch
        {
            AtataContextScope.Test => scopes.HasFlag(AtataContextScopes.Test),
            AtataContextScope.TestSuite => scopes.HasFlag(AtataContextScopes.TestSuite),
            AtataContextScope.TestSuiteGroup => scopes.HasFlag(AtataContextScopes.TestSuiteGroup),
            AtataContextScope.Namespace => scopes.HasFlag(AtataContextScopes.Namespace),
            AtataContextScope.Global => scopes.HasFlag(AtataContextScopes.Global),
            _ => false
        };

    internal static bool Satisfies(this AtataContextScopes scopes, AtataContextScope? scope) =>
        scope is null
            ? scopes == AtataContextScopes.All
            : scopes.Contains(scope.Value);
}
