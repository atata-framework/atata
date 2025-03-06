namespace Atata.NUnit;

public static class NUnitAtataContextCompletionHandler
{
    public static void Complete(AtataContext context)
    {
        if (context is not null)
        {
            var testResult = TestContext.CurrentContext.Result;

            if (testResult.Outcome.Status == TestStatus.Failed && testResult.Outcome.Site != FailureSite.Child && testResult.Outcome.Site != FailureSite.Parent)
                context.HandleTestResultException(testResult.Message, testResult.StackTrace);

            context.Dispose();
        }
    }
}
