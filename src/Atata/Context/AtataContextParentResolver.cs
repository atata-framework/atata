namespace Atata;

internal static class AtataContextParentResolver
{
    internal static AtataContext FindParentContext(AtataContext rootContext, AtataContextScope scope, TestInfo testInfo)
    {
        if (scope == AtataContextScope.Global)
            return null;

        if (rootContext is null)
            return null;

        if (rootContext.ChildContexts.Count == 0)
        {
            return rootContext;
        }
        else if (scope == AtataContextScope.NamespaceSuite)
        {
            if (testInfo.Namespace is null)
                return rootContext;
            var namespaceContext = FindNamespaceContext(rootContext, testInfo) ?? rootContext;

            if (namespaceContext != rootContext && namespaceContext.Test.Namespace == testInfo.Namespace)
                throw new InvalidOperationException(
                    $"Cannot build {nameof(AtataContext)} with scope {scope} and namespace \"{testInfo.Namespace}\", as there is already built {namespaceContext} for the same namespace.");
            else
                return namespaceContext;
        }
        else if (scope == AtataContextScope.TestSuiteGroup)
        {
            return testInfo.Namespace is null
                ? rootContext
                : FindNamespaceContext(rootContext, testInfo) ?? rootContext;
        }
        else if (scope == AtataContextScope.TestSuite)
        {
            var namespaceContext = testInfo.Namespace is null
                ? rootContext
                : FindNamespaceContext(rootContext, testInfo) ?? rootContext;

            return testInfo.SuiteGroupName is null
                ? namespaceContext
                : FindTestSuiteGroupContext(namespaceContext, testInfo) ?? namespaceContext;
        }
        else if (scope == AtataContextScope.Test)
        {
            if (testInfo.Namespace is null)
                return rootContext;

            var namespaceContext = FindNamespaceContext(rootContext, testInfo) ?? rootContext;
            var testSuiteContext = FindTestSuiteContext(namespaceContext, testInfo);

            if (testSuiteContext is not null)
                return testSuiteContext;

            if (testInfo.SuiteGroupName is null)
                return namespaceContext;

            var testSuiteGroupContext = FindTestSuiteGroupContext(namespaceContext, testInfo);

            if (testSuiteGroupContext is not null)
                return testSuiteGroupContext;
        }

        return null;
    }

    private static AtataContext FindNamespaceContext(AtataContext parentContext, TestInfo testInfo)
    {
        if (parentContext.Scope is null or < AtataContextScope.NamespaceSuite)
            return null;

        if (testInfo.Namespace == parentContext.Test.Namespace)
            return parentContext;

        if (!(parentContext.Test.Namespace is null && parentContext.ParentContext is null) && !testInfo.BelongsToNamespace(parentContext.Test.Namespace))
            return null;

        foreach (var childContext in parentContext.ChildContexts)
        {
            var childContextMatch = FindNamespaceContext(childContext, testInfo);

            if (childContextMatch is not null)
                return childContextMatch;
        }

        return parentContext;
    }

    private static AtataContext FindTestSuiteGroupContext(AtataContext parentContext, TestInfo testInfo)
    {
        foreach (var childContext in parentContext.ChildContexts)
        {
            if (childContext.Scope == AtataContextScope.TestSuiteGroup)
            {
                if (childContext.Test.SuiteGroupName == testInfo.SuiteGroupName)
                {
                    return childContext;
                }
                else
                {
                    var childContextMatch = FindTestSuiteGroupContext(childContext, testInfo);

                    if (childContextMatch is not null)
                        return childContextMatch;
                }
            }
        }

        return null;
    }

    private static AtataContext FindTestSuiteContext(AtataContext parentContext, TestInfo testInfo)
    {
        foreach (var childContext in parentContext.ChildContexts)
        {
            if (childContext.Scope == AtataContextScope.TestSuiteGroup)
            {
                var childContextMatch = FindTestSuiteContext(childContext, testInfo);

                if (childContextMatch is not null)
                    return childContextMatch;
            }
            else if (childContext.Scope == AtataContextScope.TestSuite && childContext.Test.SuiteType == testInfo.SuiteType && childContext.Test.SuiteName == testInfo.SuiteName)
            {
                return childContext;
            }
        }

        return null;
    }
}
