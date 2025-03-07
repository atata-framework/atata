namespace Atata.NUnit;

internal static class ResultStateExtensions
{
    internal static bool IsCurrentTestFailed(this ResultState resultState) =>
        resultState.Status == TestStatus.Failed && resultState.Site != FailureSite.Child;
}
