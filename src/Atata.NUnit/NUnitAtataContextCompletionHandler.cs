namespace Atata.NUnit;

public static class NUnitAtataContextCompletionHandler
{
    public static void Complete(AtataContext context)
    {
        if (context is not null)
        {
            var testResult = TestContext.CurrentContext.Result;

            if (testResult.Outcome.Status == TestStatus.Failed)
            {
                if (testResult.Outcome.Site is not FailureSite.Child and not FailureSite.Parent)
                    context.HandleTestResultException(testResult.Message, testResult.StackTrace);
                else
                    context.SetTestResultStatus(TestResultStatus.Failed);
            }
            else if (testResult.Outcome.Status == TestStatus.Inconclusive)
            {
                context.SetInconclusiveTestResult(testResult.Message);
            }

            context.Dispose();
        }
    }
}
