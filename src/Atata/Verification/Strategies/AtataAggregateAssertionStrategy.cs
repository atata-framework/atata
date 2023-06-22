using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents native/default Atata aggregate assertion strategy.
    /// </summary>
    public class AtataAggregateAssertionStrategy : IAggregateAssertionStrategy
    {
        public void Assert(Action action)
        {
            AtataContext context = AtataContext.Current;

            IEnumerable<AssertionResult> assertionResultsBefore = AtataContext.Current.PendingFailureAssertionResults.ToArray();

            try
            {
                action();
            }
            catch (Exception exception)
            {
                var failedResults = ExtractAndRemoveExclusiveFailedAssertionResults(assertionResultsBefore)
                    .Concat(new[] { AssertionResult.ForException(exception) });

                throw VerificationUtils.CreateAggregateAssertionException(failedResults);
            }

            if (context.AggregateAssertionLevel == 0)
            {
                var failedResults = ExtractAndRemoveExclusiveFailedAssertionResults(assertionResultsBefore);

                if (failedResults.Any())
                    throw VerificationUtils.CreateAggregateAssertionException(failedResults);
            }
        }

        private static IEnumerable<AssertionResult> ExtractAndRemoveExclusiveFailedAssertionResults(IEnumerable<AssertionResult> assertionResultsBefore)
        {
            var allAssertionResults = AtataContext.Current.PendingFailureAssertionResults;

            IEnumerable<AssertionResult> exclusiveAssertions = allAssertionResults
                .Where(x => !assertionResultsBefore.Contains(x))
                .Where(x => x.Status is AssertionStatus.Failed or AssertionStatus.Warning)
                .ToArray();

            if (exclusiveAssertions.Any())
                allAssertionResults.RemoveAll(x => exclusiveAssertions.Contains(x));

            return exclusiveAssertions;
        }

        public void ReportFailure(string message, string stackTrace) =>
            AtataContext.Current.PendingFailureAssertionResults.Add(AssertionResult.ForFailure(message, stackTrace));
    }
}
