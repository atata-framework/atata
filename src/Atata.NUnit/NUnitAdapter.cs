namespace Atata.NUnit;

internal static class NUnitAdapter
{
    private static readonly HashSet<string> s_internalProperties = new(StringComparer.OrdinalIgnoreCase)
        { "Author", "ApartmentState", "Description", "IgnoreUntilDate", "LevelOfParallelism", "MaxTime", "Order", "ParallelScope", "Repeat", "RequiresThread", "SetCulture", "SetUICulture", "TestOf", "Timeout" };

    internal static Test GetCurrentTest() =>
        TestExecutionContext.CurrentContext.CurrentTest;

    internal static string? GetCurrentTestName()
    {
        Test testItem = GetCurrentTest();

        return (testItem as TestMethod)?.Name;
    }

    internal static string? GetCurrentTestFixtureName()
    {
        ITest? testItem = GetCurrentTest();

        if (testItem is SetUpFixture)
            return testItem.TypeInfo?.Type.Name;

        do
        {
            if (testItem is TestFixture)
                return testItem.Name;

            testItem = testItem.Parent;
        }
        while (testItem is not null);

        return null;
    }

    internal static Type? GetCurrentTestFixtureType()
    {
        ITest? testItem = GetCurrentTest();

        if (testItem is SetUpFixture)
            return testItem.TypeInfo?.Type;

        do
        {
            if (testItem is TestFixture)
                return testItem.TypeInfo?.Type;

            testItem = testItem.Parent;
        }
        while (testItem is not null);

        return null;
    }

    internal static void AssertMultiple(Action action)
    {
        using (Assert.EnterMultipleScope())
            action.Invoke();
    }

    internal static void RecordAssertionIntoTestResult(
        NUnitAssertionStatus status,
        string message,
        string? stackTrace)
    {
        TestResult testResult = GetCurrentTestResult();

        testResult.RecordAssertion(status, message, stackTrace);
    }

    internal static void RecordTestCompletionIntoTestResult()
    {
        TestResult testResult = GetCurrentTestResult();
        testResult.RecordTestCompletion();
    }

    internal static string GetTestResultMessage() =>
        GetCurrentTestResult().Message;

    internal static TestResult GetCurrentTestResult() =>
        TestExecutionContext.CurrentContext.CurrentResult;

    internal static IReadOnlyList<TestTrait> GetCurrentTestTraits()
    {
        var properties = GetCurrentTest().Properties;

        var keys = properties.Keys;
        List<TestTrait> traits = [];

        foreach (var key in keys)
        {
            if (!s_internalProperties.Contains(key) && properties.TryGet(key, out var values))
            {
                foreach (object value in values)
                {
                    traits.Add(new TestTrait(key, value?.ToString()));
                }
            }
        }

        return traits;
    }
}
