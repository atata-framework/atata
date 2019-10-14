using System;

namespace Atata
{
    /// <summary>
    /// Represents aggregate assertion strategy for NUnit.
    /// Uses NUnit's <c>Assert.Multiple</c> method for aggregate assertion.
    /// </summary>
    public class NUnitAggregateAssertionStrategy : IAggregateAssertionStrategy
    {
        public void Assert(Action action)
        {
            NUnitAdapter.AssertMultiple(action);
        }

        public void ReportFailure(string message, string stackTrace)
        {
            NUnitAdapter.RecordAssertionIntoTestResult(NUnitAdapter.AssertionStatus.Failed, message, stackTrace);
            NUnitAdapter.RecordTestCompletionIntoTestResult();
        }
    }
}
