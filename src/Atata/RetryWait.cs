﻿#nullable enable

namespace Atata;

/// <summary>
/// Represents a wait class that allows to retry a condition until it succeeds or the timeout is reached.
/// Allows to ignore specific types of exceptions while waiting for a condition.
/// </summary>
public sealed class RetryWait
{
    private readonly TimeSpan _timeout;

    private readonly TimeSpan _retryInterval;

    private readonly List<Type> _ignoredExceptionTypes = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="RetryWait"/> class.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <param name="retryInterval">The retry interval.</param>
    public RetryWait(TimeSpan timeout, TimeSpan retryInterval)
    {
        _timeout = timeout;
        _retryInterval = retryInterval;
    }

    /// <summary>
    /// Configures this instance to ignore the specified <typeparamref name="TException"/> type of exception while waiting for a condition.
    /// Any exceptions not white-listed will be allowed to propagate, terminating the wait.
    /// </summary>
    /// <typeparam name="TException">The type of the exception.</typeparam>
    /// <returns>The same <see cref="RetryWait"/> instance.</returns>
    public RetryWait Ignore<TException>()
        where TException : Exception
    {
        _ignoredExceptionTypes.Add(typeof(TException));
        return this;
    }

    /// <summary>
    /// Configures this instance to ignore specific types of exceptions while waiting for a condition.
    /// Any exceptions not white-listed will be allowed to propagate, terminating the wait.
    /// </summary>
    /// <param name="exceptionTypes">The types of exceptions to ignore.</param>
    /// <returns>The same <see cref="RetryWait"/> instance.</returns>
    public RetryWait IgnoreExceptions(params Type[] exceptionTypes)
    {
        exceptionTypes.CheckNotNull(nameof(exceptionTypes));

        _ignoredExceptionTypes.AddRange(exceptionTypes);
        return this;
    }

    /// <summary>
    /// Configures this instance to ignore all types of exceptions while waiting for a condition.
    /// </summary>
    /// <returns>The same <see cref="RetryWait"/> instance.</returns>
    public RetryWait IgnoreAllExceptions() =>
        IgnoreExceptions(typeof(Exception));

    /// <inheritdoc cref="Until(Func{bool})"/>
    /// <param name="condition">A delegate to be executed over and over until it returns <see langword="true"/>.</param>
    /// <param name="input">The input value to pass to the condition.</param>
    public bool Until<TInput>(Func<TInput, bool> condition, TInput input)
    {
        condition.CheckNotNull(nameof(condition));
        input.CheckNotNull(nameof(input));

        return Until(() => condition.Invoke(input));
    }

    /// <summary>
    /// Waits until the specified <paramref name="condition"/> is satisfied or until the timeout is expired.
    /// </summary>
    /// <param name="condition">A delegate to be executed over and over until it returns <see langword="true"/>.</param>
    /// <returns><see langword="true"/> if the condition is satisfied within the timeout; otherwise, <see langword="false"/>.</returns>
    public bool Until(Func<bool> condition)
    {
        condition.CheckNotNull(nameof(condition));

        long operationStart = Stopwatch.GetTimestamp();
        long operationTimeoutEnd = operationStart + _timeout.Ticks;

        while (true)
        {
            long iterationStart = Stopwatch.GetTimestamp();

            try
            {
                var succeeded = condition.Invoke();

                if (succeeded)
                    return true;
            }
            catch (Exception exception)
            {
                if (!IsIgnoredException(exception))
                    throw;
            }

            long iterationEnd = Stopwatch.GetTimestamp();
            long ticksUntilTimeout = operationTimeoutEnd - iterationEnd;

            if (ticksUntilTimeout <= 0)
            {
                return false;
            }
            else
            {
                long ticksToSleep = _retryInterval.Ticks - (iterationEnd - iterationStart);

                if (ticksUntilTimeout < ticksToSleep)
                    ticksToSleep = ticksUntilTimeout;

                if (ticksToSleep > 0)
                    Thread.Sleep(new TimeSpan(ticksToSleep));
            }
        }
    }

    /// <inheritdoc cref="UntilAsync(Func{CancellationToken, Task{bool}}, CancellationToken)"/>
    public async Task<bool> UntilAsync(Func<Task<bool>> condition, CancellationToken cancellationToken = default)
    {
        condition.CheckNotNull(nameof(condition));

        return await UntilAsync(_ => condition.Invoke(), cancellationToken);
    }

    /// <inheritdoc cref="UntilAsync{TInput}(Func{TInput, CancellationToken, Task{bool}}, TInput, CancellationToken)"/>
    public async Task<bool> UntilAsync<TInput>(Func<TInput, Task<bool>> condition, TInput input, CancellationToken cancellationToken = default)
    {
        condition.CheckNotNull(nameof(condition));
        input.CheckNotNull(nameof(input));

        return await UntilAsync(_ => condition.Invoke(input), cancellationToken);
    }

    /// <inheritdoc cref="UntilAsync(Func{CancellationToken, Task{bool}}, CancellationToken)"/>
    public async Task<bool> UntilAsync(Func<bool> condition, CancellationToken cancellationToken = default)
    {
        condition.CheckNotNull(nameof(condition));

        return await UntilAsync(_ => Task.FromResult(condition.Invoke()), cancellationToken);
    }

    /// <inheritdoc cref="UntilAsync{TInput}(Func{TInput, CancellationToken, Task{bool}}, TInput, CancellationToken)"/>
    public async Task<bool> UntilAsync<TInput>(Func<TInput, bool> condition, TInput input, CancellationToken cancellationToken = default)
    {
        condition.CheckNotNull(nameof(condition));
        input.CheckNotNull(nameof(input));

        return await UntilAsync(_ => Task.FromResult(condition.Invoke(input)), cancellationToken);
    }

    /// <inheritdoc cref="Until(Func{bool})"/>
    /// <param name="condition">A delegate to be executed over and over until it returns <see langword="true"/>.</param>
    /// <param name="input">The input value to pass to the condition.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task<bool> UntilAsync<TInput>(Func<TInput, CancellationToken, Task<bool>> condition, TInput input, CancellationToken cancellationToken = default)
    {
        condition.CheckNotNull(nameof(condition));
        input.CheckNotNull(nameof(input));

        return await UntilAsync(ct => condition.Invoke(input, ct), cancellationToken);
    }

    /// <inheritdoc cref="Until(Func{bool})"/>
    /// <param name="condition">A delegate to be executed over and over until it returns <see langword="true"/>.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task<bool> UntilAsync(Func<CancellationToken, Task<bool>> condition, CancellationToken cancellationToken = default)
    {
        condition.CheckNotNull(nameof(condition));

        long operationStart = Stopwatch.GetTimestamp();
        long operationTimeoutEnd = operationStart + _timeout.Ticks;

        while (true)
        {
            long iterationStart = Stopwatch.GetTimestamp();

            try
            {
                var succeeded = await condition.Invoke(cancellationToken).ConfigureAwait(false);

                if (succeeded)
                    return true;
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                if (!IsIgnoredException(exception))
                    throw;
            }

            long iterationEnd = Stopwatch.GetTimestamp();
            long ticksUntilTimeout = operationTimeoutEnd - iterationEnd;

            if (ticksUntilTimeout <= 0)
            {
                return false;
            }
            else
            {
                long ticksToSleep = _retryInterval.Ticks - (iterationEnd - iterationStart);

                if (ticksUntilTimeout < ticksToSleep)
                    ticksToSleep = ticksUntilTimeout;

                if (ticksToSleep > 0)
                    await Task.Delay(new TimeSpan(ticksToSleep), cancellationToken).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Creates the <see cref="TimeoutException"/> with a message in a following format:
    /// <c>"Timed out after {time} waiting for {expectation}."</c>.
    /// </summary>
    /// <param name="expectation">The expectation text.</param>
    /// <returns>The <see cref="TimeoutException"/> created.</returns>
    public TimeoutException CreateTimeoutExceptionFor(string expectation) =>
        new($"Timed out after {_timeout.ToShortIntervalString()} waiting for {expectation}.");

    private bool IsIgnoredException(Exception exception) =>
        _ignoredExceptionTypes.Exists(type => type.IsAssignableFrom(exception.GetType()));
}