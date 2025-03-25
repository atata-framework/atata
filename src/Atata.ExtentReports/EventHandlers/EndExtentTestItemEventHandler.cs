namespace Atata.ExtentReports;

public sealed class EndExtentTestItemEventHandler : IEventHandler<AtataContextDeInitCompletedEvent>
{
    public void Handle(AtataContextDeInitCompletedEvent eventData, AtataContext context)
    {
        ExtentContext extentContext = ExtentContext.ResolveFor(context);

        if (extentContext is not null)
        {
            extentContext.Test.Test.EndTime = DateTime.Now;
            extentContext.Test.Test.Status = ConvertTestResultStatusToExtentStatus(context.TestResultStatus);
        }
    }

    private static Status ConvertTestResultStatusToExtentStatus(TestResultStatus testResultStatus) =>
        testResultStatus switch
        {
            TestResultStatus.Inconclusive => Status.Skip,
            TestResultStatus.Warning => Status.Warning,
            TestResultStatus.Failed => Status.Fail,
            _ => Status.Pass,
        };
}
