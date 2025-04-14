namespace Atata;

/// <summary>
/// <para>
/// A factory that creates a relative artifacts path for a given <see cref="AtataContext"/>
/// in a form of:
/// </para>
/// <list type="bullet">
/// <item>For test: <c>"SomeRelativeNamespace/SomeTestClass/SomeTest"</c></item>
/// <item>For test suite: <c>"SomeRelativeNamespace/SomeTestClass"</c></item>
/// <item>For test suite group: <c>"SomeRelativeNamespace/SomeFixtureClass"</c></item>
/// <item>For namespace: <c>"SomeRelativeNamespace"</c></item>
/// <item>For global: <c>""</c></item>
/// </list>
/// <para>
/// There is a possibility to specify a sub-folder name for "suite" contexts.
/// For example, if you specify <c>"_"</c> as a constructor argument, the form of the paths will be:
/// </para>
/// <list type="bullet">
/// <item>For test: <c>"SomeRelativeNamespace/SomeTestClass/SomeTest"</c></item>
/// <item>For test suite: <c>"SomeRelativeNamespace/SomeTestClass/_"</c></item>
/// <item>For test suite group: <c>"SomeRelativeNamespace/_SomeFixtureClass"</c></item>
/// <item>For namespace: <c>"SomeRelativeNamespace/_"</c></item>
/// <item>For global: <c>"_"</c></item>
/// </list>
/// </summary>
public class TestInfoBasedHierarchicalArtifactsPathFactory : IArtifactsPathFactory
{
    private readonly bool _hasSuiteSubFolderName;

    private readonly string? _suiteSubFolderName;

    private readonly string? _suiteSubFolderPathEnding;

    public TestInfoBasedHierarchicalArtifactsPathFactory()
        : this(null)
    {
    }

    public TestInfoBasedHierarchicalArtifactsPathFactory(string? suiteSubFolderName)
    {
        _hasSuiteSubFolderName = !string.IsNullOrEmpty(suiteSubFolderName);
        _suiteSubFolderName = suiteSubFolderName;
        _suiteSubFolderPathEnding = $"/{_suiteSubFolderName}";
    }

    /// <inheritdoc/>
    public string Create(AtataContext context)
    {
        if (context.Scope is null || context.Test.Namespace is null)
        {
            return Sanitize(context.Id);
        }

        return context.Scope switch
        {
            AtataContextScope.Global => _suiteSubFolderName ?? string.Empty,
            AtataContextScope.Namespace => CreateNamespacePath(context),
            AtataContextScope.TestSuiteGroup => CreateTestSuiteGroupPath(context),
            AtataContextScope.TestSuite => CreateTestSuitePath(context),
            AtataContextScope.Test => CreateTestPath(context),
            _ => throw Guard.CreateArgumentExceptionForUnsupportedValue(context.Scope)
        };
    }

    private static string ResolveRelativePathForSuite(TestInfo testInfo) =>
        ResolveRelativePath($"{testInfo.Namespace}.{testInfo.SuiteNameSanitized}");

    private static string ResolveRelativePath(string typeNameOrNamespace)
    {
        string? rootNamespace = AtataContext.GlobalProperties.RootNamespace;

        if (rootNamespace is not null && typeNameOrNamespace.StartsWith(rootNamespace, StringComparison.Ordinal))
            typeNameOrNamespace = typeNameOrNamespace.Length > rootNamespace.Length
                ? typeNameOrNamespace[(rootNamespace.Length + 1)..]
                : string.Empty;

        return ReplacePeriodWithSlash(typeNameOrNamespace);
    }

    private static string ReplacePeriodWithSlash(string value) =>
        value.Replace('.', '/');

    private static string Sanitize(string value) =>
        value.SanitizeForFileName('_');

    private string CreateNamespacePath(AtataContext context)
    {
        string namespacePath = ResolveRelativePath(context.Test.Namespace!);
        return _hasSuiteSubFolderName
            ? $"{namespacePath}{_suiteSubFolderPathEnding}"
            : namespacePath;
    }

    private string CreateTestSuiteGroupPath(AtataContext context)
    {
        string namespacePath = ResolveRelativePath(context.Test.Namespace!);
        return ArtifactsPathUniquenessGuarantor.EnsurePathIsUnique(
            $"{namespacePath}/{_suiteSubFolderName}{context.Test.SuiteNameSanitized}");
    }

    private string CreateTestSuitePath(AtataContext context)
    {
        string namespacePath = ResolveRelativePath(context.Test.Namespace!);

        string namespaceAndSuiteNamePath = ArtifactsPathUniquenessGuarantor.EnsurePathIsUnique(
            $"{namespacePath}/{context.Test.SuiteNameSanitized}");

        return _hasSuiteSubFolderName
            ? $"{namespaceAndSuiteNamePath}{_suiteSubFolderPathEnding}"
            : namespaceAndSuiteNamePath;
    }

    private string CreateTestPath(AtataContext context)
    {
        string testSuitePath = context.ParentContext is { Scope: AtataContextScope.TestSuite }
            ? GetTestSuiteArtifactsPath(context.ParentContext)
            : ResolveRelativePathForSuite(context.Test);

        string? testName = context.Test.NameSanitized;

        string path = string.IsNullOrWhiteSpace(testName)
            ? $"{testSuitePath}/{Sanitize(context.Id)}"
            : $"{testSuitePath}/{testName}";

        return ArtifactsPathUniquenessGuarantor.EnsurePathIsUnique(path);
    }

    private string GetTestSuiteArtifactsPath(AtataContext context)
    {
        string path = context.ArtifactsRelativePath;

        return _hasSuiteSubFolderName && path.EndsWith(_suiteSubFolderPathEnding, StringComparison.Ordinal)
            ? path[..^_suiteSubFolderPathEnding!.Length]
            : path;
    }
}
