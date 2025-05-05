namespace Atata.MSTest;

/// <summary>
/// Provides methods to handle the completion of <see cref="AtataContext"/> in MSTest tests.
/// </summary>
public static class MSTestAtataContextCompletionHandler
{
    /// <summary>
    /// Completes the <see cref="AtataContext"/> for the specified test context.
    /// If an <see cref="AtataContext"/> is associated with the <paramref name="testContext"/>,
    /// it is completed and removed from the test context.
    /// </summary>
    /// <param name="testContext">The MSTest <see cref="TestContext"/>.</param>
    public static void Complete(TestContext testContext)
    {
        AtataContext? atataContext = testContext.GetAtataContextOrNull();

        if (atataContext is not null)
        {
            Complete(testContext, atataContext);
            testContext.RemoveAtataContext();
        }
    }

    /// <summary>
    /// Completes the specified <paramref name="atataContext"/> for the given <paramref name="testContext"/>.
    /// Handles test result outcome and disposes the <paramref name="atataContext"/>.
    /// </summary>
    /// <param name="testContext">The MSTest <see cref="TestContext"/>.</param>
    /// <param name="atataContext">The <see cref="AtataContext"/> to complete.</param>
    public static void Complete(TestContext testContext, AtataContext atataContext)
    {
        if (atataContext is not null)
        {
            if (testContext.TestException is not null)
            {
                if (testContext.CurrentTestOutcome == UnitTestOutcome.Inconclusive)
                    atataContext.SetInconclusiveTestResult(testContext.TestException.Message);
                else
                    atataContext.HandleTestResultException(testContext.TestException);
            }

            atataContext.Dispose();
        }
    }
}
