using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Atata.NUnit;

public static class TestCompletionHandler
{
    public static void CompleteTest(AtataContext context)
    {
        if (context is not null)
        {
            var testContext = TestContext.CurrentContext;
            var testOutcome = testContext.Result.Outcome;

            if (testOutcome.Status == TestStatus.Failed && testOutcome.Site != FailureSite.Child && testOutcome.Site != FailureSite.Parent)
                context.HandleTestResultException(testContext.Result.Message, testContext.Result.StackTrace);

            context.Dispose();
        }
    }
}
