namespace Atata;

/// <summary>
/// Represents native/default Atata aggregate assertion strategy.
/// </summary>
public class AtataAggregateAssertionStrategy : IAggregateAssertionStrategy
{
    public void Assert(Action action)
    {
        AtataContext context = AtataContext.Current;

        try
        {
            action();
        }
        catch (Exception exception)
        {
            AssertionResult[] failedResults = [
                .. context.GetAndClearPendingFailureAssertionResults(),
                AssertionResult.ForException(exception)];

            throw VerificationUtils.CreateAggregateAssertionException(failedResults);
        }

        if (context.AggregateAssertionLevel == 0
            && context.PendingFailureAssertionResults.Exists(x => x.Status is AssertionStatus.Failed or AssertionStatus.Exception))
        {
            throw VerificationUtils.CreateAggregateAssertionException(
                context.GetAndClearPendingFailureAssertionResults());
        }
    }

    public void ReportFailure(string message, string stackTrace) =>
        AtataContext.Current.PendingFailureAssertionResults.Add(AssertionResult.ForFailure(message, stackTrace));
}
