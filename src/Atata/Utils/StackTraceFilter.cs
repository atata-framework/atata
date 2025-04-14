namespace Atata;

public static class StackTraceFilter
{
    public static string TakeBeforeInvokeMethodOfRuntimeMethodHandle(string stackTrace) =>
        TakeBefore(stackTrace, @" System\.RuntimeMethodHandle\.InvokeMethod");

    public static string TakeBefore(string stackTrace, string pattern)
    {
        Regex regex = new(pattern);

        return Filter(stackTrace, frames => frames.TakeWhile(x => !regex.IsMatch(x)));
    }

    public static string Filter(string stackTrace, Func<IEnumerable<string>, IEnumerable<string>> stackFramesFilter)
    {
        Guard.ThrowIfNull(stackTrace);
        Guard.ThrowIfNull(stackFramesFilter);

        IEnumerable<string> originalStackFrames = stackTrace.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

        IEnumerable<string> filteredStackFrames = stackFramesFilter.Invoke(originalStackFrames);

        return string.Join(Environment.NewLine, filteredStackFrames);
    }
}
