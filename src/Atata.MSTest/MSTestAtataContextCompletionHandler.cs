namespace Atata.MSTest;

public static class MSTestAtataContextCompletionHandler
{
    public static void Complete(TestContext testContext)
    {
        AtataContext? atataContext = testContext.GetAtataContextOrNull();

        if (atataContext is not null)
        {
            Complete(testContext, atataContext);
            testContext.RemoveAtataContext();
        }
    }

    public static void Complete(TestContext testContext, AtataContext? atataContext)
    {
        if (atataContext is not null)
        {
            if (testContext.TestException is not null)
                atataContext.HandleTestResultException(testContext.TestException);

            atataContext.Dispose();
        }
    }
}
