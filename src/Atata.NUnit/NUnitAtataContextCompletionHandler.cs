namespace Atata.NUnit;

/// <summary>
/// Provides a handler for completing the <see cref="AtataContext"/> in NUnit tests.
/// </summary>
public static class NUnitAtataContextCompletionHandler
{
    /// <summary>
    /// Completes the specified <see cref="AtataContext"/> by handling the test result and disposing of the context.
    /// </summary>
    /// <param name="context">The <see cref="AtataContext"/> to complete.</param>
    public static void Complete(AtataContext context)
    {
        if (context is not null)
        {
            var testResult = TestContext.CurrentContext.Result;

            if (testResult.Outcome.Status == TestStatus.Failed)
            {
                if (testResult.Outcome.Site is not FailureSite.Child and not FailureSite.Parent)
                    context.HandleTestResultException(testResult.Message, testResult.StackTrace);
            }
            else if (testResult.Outcome.Status == TestStatus.Inconclusive)
            {
                context.SetInconclusiveTestResult(testResult.Message);
            }

            context.Dispose();
        }
    }
}
