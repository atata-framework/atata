#nullable enable

namespace Atata;

internal static class TaskExtensions
{
    internal static void RunSync(this Task task)
    {
        if (task.Status != TaskStatus.RanToCompletion)
            task.GetAwaiter().GetResult();
    }

    internal static TResult RunSync<TResult>(this Task<TResult> task) =>
        task.Status == TaskStatus.RanToCompletion
            ? task.Result
            : task.GetAwaiter().GetResult();

    internal static void RunSync(this ValueTask valueTask)
    {
        if (!valueTask.IsCompletedSuccessfully)
            valueTask.GetAwaiter().GetResult();
    }

    internal static TResult RunSync<TResult>(this ValueTask<TResult> valueTask) =>
        valueTask.IsCompletedSuccessfully
            ? valueTask.Result
            : valueTask.GetAwaiter().GetResult();
}
