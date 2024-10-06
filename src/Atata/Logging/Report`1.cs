namespace Atata;

/// <summary>
/// Provides reporting functionality.
/// </summary>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public class Report<TOwner> : IReport<TOwner>
{
    private readonly TOwner _owner;

    private readonly IAtataExecutionUnit _executionUnit;

    /// <summary>
    /// Initializes a new instance of the <see cref="Report{TOwner}"/> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="executionUnit">The execution unit.</param>
    public Report(TOwner owner, IAtataExecutionUnit executionUnit)
    {
        _owner = owner.CheckNotNull(nameof(owner));
        _executionUnit = executionUnit.CheckNotNull(nameof(executionUnit));
    }

    /// <summary>
    /// Gets the owner.
    /// </summary>
    protected TOwner Owner => _owner;

    /// <inheritdoc/>
    public TOwner Trace(string message)
    {
        _executionUnit.Log.Trace(message);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Debug(string message)
    {
        _executionUnit.Log.Debug(message);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Info(string message)
    {
        _executionUnit.Log.Info(message);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Warn(string message)
    {
        _executionUnit.Log.Warn(message);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Warn(Exception exception)
    {
        _executionUnit.Log.Warn(exception);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Warn(Exception exception, string message)
    {
        _executionUnit.Log.Warn(exception, message);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Error(string message)
    {
        _executionUnit.Log.Error(message);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Error(Exception exception)
    {
        _executionUnit.Log.Error(exception);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Error(Exception exception, string message)
    {
        _executionUnit.Log.Error(exception, message);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Fatal(string message)
    {
        _executionUnit.Log.Fatal(message);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Fatal(Exception exception)
    {
        _executionUnit.Log.Fatal(exception);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Fatal(Exception exception, string message)
    {
        _executionUnit.Log.Fatal(exception, message);
        return _owner;
    }

    /// <inheritdoc/>
    public TOwner Setup(string message, Action<TOwner> action)
    {
        message.CheckNotNullOrEmpty(nameof(message));
        action.CheckNotNull(nameof(action));

        _executionUnit.Log.ExecuteSection(new SetupLogSection(message), () =>
        {
            bool shouldStopBodyExecutionStopwatch = _executionUnit.Context.BodyExecutionStopwatch.IsRunning;
            if (shouldStopBodyExecutionStopwatch)
                _executionUnit.Context.BodyExecutionStopwatch.Stop();

            _executionUnit.Context.SetupExecutionStopwatch.Start();

            try
            {
                action.Invoke(_owner);
            }
            catch (Exception exception)
            {
                _executionUnit.Context.EnsureExceptionIsLogged(exception);
                throw;
            }
            finally
            {
                _executionUnit.Context.SetupExecutionStopwatch.Stop();

                if (shouldStopBodyExecutionStopwatch)
                    _executionUnit.Context.BodyExecutionStopwatch.Start();
            }
        });
        return _owner;
    }

    /// <inheritdoc/>
    public TResult Setup<TResult>(string message, Func<TOwner, TResult> function)
    {
        message.CheckNotNullOrEmpty(nameof(message));
        function.CheckNotNull(nameof(function));

        TResult result = default;

        _executionUnit.Log.ExecuteSection(new SetupLogSection(message), () =>
        {
            bool shouldStopBodyExecutionStopwatch = _executionUnit.Context.BodyExecutionStopwatch.IsRunning;
            if (shouldStopBodyExecutionStopwatch)
                _executionUnit.Context.BodyExecutionStopwatch.Stop();

            _executionUnit.Context.SetupExecutionStopwatch.Start();

            try
            {
                result = function.Invoke(_owner);
            }
            catch (Exception exception)
            {
                _executionUnit.Context.EnsureExceptionIsLogged(exception);
                throw;
            }
            finally
            {
                _executionUnit.Context.SetupExecutionStopwatch.Stop();

                if (shouldStopBodyExecutionStopwatch)
                    _executionUnit.Context.BodyExecutionStopwatch.Start();
            }
        });

        return result;
    }

    /// <inheritdoc/>
    public async Task SetupAsync(string message, Func<TOwner, Task> function)
    {
        message.CheckNotNullOrEmpty(nameof(message));
        function.CheckNotNull(nameof(function));

        await _executionUnit.Log.ExecuteSectionAsync(new SetupLogSection(message), async () =>
        {
            bool shouldStopBodyExecutionStopwatch = _executionUnit.Context.BodyExecutionStopwatch.IsRunning;
            if (shouldStopBodyExecutionStopwatch)
                _executionUnit.Context.BodyExecutionStopwatch.Stop();

            _executionUnit.Context.SetupExecutionStopwatch.Start();

            try
            {
                await function.Invoke(_owner);
            }
            catch (Exception exception)
            {
                _executionUnit.Context.EnsureExceptionIsLogged(exception);
                throw;
            }
            finally
            {
                _executionUnit.Context.SetupExecutionStopwatch.Stop();

                if (shouldStopBodyExecutionStopwatch)
                    _executionUnit.Context.BodyExecutionStopwatch.Start();
            }
        });
    }

    /// <inheritdoc/>
    public async Task<TResult> SetupAsync<TResult>(string message, Func<TOwner, Task<TResult>> function)
    {
        message.CheckNotNullOrEmpty(nameof(message));
        function.CheckNotNull(nameof(function));

        TResult result = default;

        await _executionUnit.Log.ExecuteSectionAsync(new SetupLogSection(message), async () =>
        {
            bool shouldStopBodyExecutionStopwatch = _executionUnit.Context.BodyExecutionStopwatch.IsRunning;
            if (shouldStopBodyExecutionStopwatch)
                _executionUnit.Context.BodyExecutionStopwatch.Stop();

            _executionUnit.Context.SetupExecutionStopwatch.Start();

            try
            {
                result = await function.Invoke(_owner);
            }
            catch (Exception exception)
            {
                _executionUnit.Context.EnsureExceptionIsLogged(exception);
                throw;
            }
            finally
            {
                _executionUnit.Context.SetupExecutionStopwatch.Stop();

                if (shouldStopBodyExecutionStopwatch)
                    _executionUnit.Context.BodyExecutionStopwatch.Start();
            }
        });

        return result;
    }

    /// <inheritdoc/>
    public TOwner Step(string message, Action<TOwner> action)
    {
        message.CheckNotNullOrEmpty(nameof(message));
        action.CheckNotNull(nameof(action));

        _executionUnit.Log.ExecuteSection(new StepLogSection(message), () =>
        {
            try
            {
                action.Invoke(_owner);
            }
            catch (Exception exception)
            {
                _executionUnit.Context.EnsureExceptionIsLogged(exception);
                throw;
            }
        });
        return _owner;
    }

    /// <inheritdoc/>
    public TResult Step<TResult>(string message, Func<TOwner, TResult> function)
    {
        message.CheckNotNullOrEmpty(nameof(message));
        function.CheckNotNull(nameof(function));

        TResult result = default;

        _executionUnit.Log.ExecuteSection(new StepLogSection(message), () =>
        {
            try
            {
                result = function.Invoke(_owner);
            }
            catch (Exception exception)
            {
                _executionUnit.Context.EnsureExceptionIsLogged(exception);
                throw;
            }
        });

        return result;
    }

    /// <inheritdoc/>
    public async Task StepAsync(string message, Func<TOwner, Task> function)
    {
        message.CheckNotNullOrEmpty(nameof(message));
        function.CheckNotNull(nameof(function));

        await _executionUnit.Log.ExecuteSectionAsync(new StepLogSection(message), async () =>
        {
            try
            {
                await function.Invoke(_owner);
            }
            catch (Exception exception)
            {
                _executionUnit.Context.EnsureExceptionIsLogged(exception);
                throw;
            }
        });
    }

    /// <inheritdoc/>
    public async Task<TResult> StepAsync<TResult>(string message, Func<TOwner, Task<TResult>> function)
    {
        message.CheckNotNullOrEmpty(nameof(message));
        function.CheckNotNull(nameof(function));

        TResult result = default;

        await _executionUnit.Log.ExecuteSectionAsync(new StepLogSection(message), async () =>
        {
            try
            {
                result = await function.Invoke(_owner);
            }
            catch (Exception exception)
            {
                _executionUnit.Context.EnsureExceptionIsLogged(exception);
                throw;
            }
        });

        return result;
    }
}
