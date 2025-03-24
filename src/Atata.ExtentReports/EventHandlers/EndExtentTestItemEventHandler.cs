namespace Atata.ExtentReports;

public sealed class EndExtentTestItemEventHandler : IEventHandler<AtataContextDeInitCompletedEvent>
{
    public void Handle(AtataContextDeInitCompletedEvent eventData, AtataContext context)
    {
        ExtentContext extentContext = ExtentContext.ResolveFor(context);

        if (extentContext is not null)
        {
            extentContext.Test.Test.EndTime = DateTime.Now;
            extentContext.Test.Test.Status = ResolveCurrentTestStatus(context.Test.ResultStatus);
        }
    }

    private static Status ResolveCurrentTestStatus(TestResultStatus testResultStatus) =>
        testResultStatus switch
        {
            // TODO: Handle Inconclusive state when it is added to TestResultStatus.
            ////TestResultStatus.Inconclusive or TestResultStatus.Skipped => Status.Skip,
            TestResultStatus.Warning => Status.Warning,
            TestResultStatus.Failed => Status.Fail,
            _ => Status.Pass,
        };
}
