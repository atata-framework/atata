namespace Atata.NUnit;

internal static class NUnitAdapter
{
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
}
