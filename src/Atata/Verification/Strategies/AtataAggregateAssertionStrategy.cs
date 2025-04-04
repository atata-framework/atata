namespace Atata;

/// <summary>
/// Represents native/default Atata aggregate assertion strategy.
/// </summary>
public sealed class AtataAggregateAssertionStrategy : IAggregateAssertionStrategy
{
    public static AtataAggregateAssertionStrategy Instance { get; } = new();

    public void Assert(IAtataExecutionUnit? executionUnit, Action action)
    {
        AtataContext context = executionUnit?.Context ?? AtataContext.ResolveCurrent();

        try
        {
            action();
        }
        catch (Exception exception)
        {
            AssertionResult[] failedResults = [
                .. context.GetAndClearPendingFailureAssertionResults(),
                AssertionResult.ForException(exception)];

            throw VerificationUtils.CreateAggregateAssertionException(context, failedResults);
        }

        if (context.AggregateAssertionLevel == 0
            && context.PendingFailureAssertionResults.Exists(x => x.Status is AssertionStatus.Failed or AssertionStatus.Exception))
        {
            throw VerificationUtils.CreateAggregateAssertionException(
                context,
                context.GetAndClearPendingFailureAssertionResults());
        }
    }

    public void ReportFailure(IAtataExecutionUnit? executionUnit, string message, string stackTrace)
    {
        AtataContext context = executionUnit?.Context ?? AtataContext.ResolveCurrent();
        context.PendingFailureAssertionResults.Add(AssertionResult.ForFailure(message, stackTrace));
    }
}
