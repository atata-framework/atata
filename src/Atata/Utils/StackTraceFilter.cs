#nullable enable

namespace Atata;

public static class StackTraceFilter
{
    public static string TakeBeforeInvokeMethodOfRuntimeMethodHandle(string stackTrace) =>
        TakeBefore(stackTrace, @" System\.RuntimeMethodHandle\.InvokeMethod");

    public static string TakeBefore(string stackTrace, string pattern)
    {
        Regex regex = new Regex(pattern);

        return Filter(stackTrace, frames => frames.TakeWhile(x => !regex.IsMatch(x)));
    }

    public static string Filter(string stackTrace, Func<IEnumerable<string>, IEnumerable<string>> stackFramesFilter)
    {
        stackTrace.CheckNotNull(nameof(stackTrace));
        stackFramesFilter.CheckNotNull(nameof(stackFramesFilter));

        IEnumerable<string> originalStackFrames = stackTrace.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

        IEnumerable<string> filteredStackFrames = stackFramesFilter.Invoke(originalStackFrames);

        return string.Join(Environment.NewLine, filteredStackFrames);
    }
}
