namespace Atata;

public static class TestResultStatusConditionExtensions
{
    public static bool DoesMeet(this TestResultStatusCondition skipCondition, TestResultStatus testResultStatus) =>
        skipCondition switch
        {
            TestResultStatusCondition.None => true,
            TestResultStatusCondition.Passed => testResultStatus == TestResultStatus.Passed,
            TestResultStatusCondition.PassedOrInconclusive => testResultStatus is TestResultStatus.Passed or TestResultStatus.Inconclusive,
            TestResultStatusCondition.PassedOrInconclusiveOrWarning => testResultStatus is TestResultStatus.Passed or TestResultStatus.Inconclusive or TestResultStatus.Warning,
            _ => throw Guard.CreateArgumentExceptionForUnsupportedValue(skipCondition)
        };
}
