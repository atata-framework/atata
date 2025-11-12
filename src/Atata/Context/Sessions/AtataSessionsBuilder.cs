namespace Atata;

/// <summary>
/// A builder of Atata sessions.
/// </summary>
public sealed class AtataSessionsBuilder
{
    private readonly AtataContextBuilder _atataContextBuilder;

    private readonly List<IAtataSessionProvider> _sessionProviders;

    private readonly AtataContextScopes? _defaultStartScopes;

    internal AtataSessionsBuilder(
        AtataContextBuilder atataContextBuilder,
        List<IAtataSessionProvider> sessionProviders,
        AtataContextScopes? defaultStartScopes)
    {
        _atataContextBuilder = atataContextBuilder;
        _sessionProviders = sessionProviders;
        _defaultStartScopes = defaultStartScopes;
    }

    /// <summary>
    /// Gets only session builders out of registered providers.
    /// </summary>
    public IEnumerable<IAtataSessionBuilder> Builders =>
        _sessionProviders.OfType<IAtataSessionBuilder>();

    /// <summary>
    /// Gets all the registered session providers: builders, borrow requests, pool requests.
    /// </summary>
    public IReadOnlyList<IAtataSessionProvider> Providers =>
        _sessionProviders;

    internal AtataContextScopes? DefaultStartScopes =>
        _defaultStartScopes;

    public IEnumerable<IAtataSessionProvider> GetProvidersForScope(AtataContextScope? scope)
    {
        foreach (var provider in _sessionProviders)
            if (DoesSessionStartScopeSatisfyContextScope(provider.StartScopes, scope))
                yield return provider;
    }

    /// <summary>
    /// Creates a new instance of the builder of the specified <typeparamref name="TSessionBuilder"/> type,
    /// calls <paramref name="configure"/> delegate,
    /// adds it to the session providers list.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Add<TSessionBuilder>(Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder, new()
    {
        var sessionBuilder = new TSessionBuilder
        {
            StartScopes = _defaultStartScopes
        };
        configure?.Invoke(sessionBuilder);

        return Add(sessionBuilder);
    }

    /// <summary>
    /// Adds the specified session provider.
    /// </summary>
    /// <param name="sessionProvider">The session provider.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Add(IAtataSessionProvider sessionProvider)
    {
        Guard.ThrowIfNull(sessionProvider);

        _sessionProviders.Add(sessionProvider);
        return _atataContextBuilder;
    }

    /// <summary>
    /// Configures existing nameless <typeparamref name="TSessionBuilder"/> session builder.
    /// The <paramref name="mode"/> (<see cref="ConfigurationMode.ConfigureOrThrow"/> by default)
    /// parameter specifies the behavior of the fallback logic when the session builder is not found:
    /// <list type="bullet">
    /// <item><see cref="ConfigurationMode.ConfigureOrThrow"/> - configures the builder or throws the <see cref="AtataSessionBuilderNotFoundException"/> if it is not found.</item>
    /// <item><see cref="ConfigurationMode.ConfigureIfExists"/> - configures the builder only if it exists; otherwise, no action is taken.</item>
    /// <item><see cref="ConfigurationMode.ConfigureOrAdd"/> - configures the builder if it exists, or adds a new builder if it does not exist.</item>
    /// </list>
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TSessionBuilder"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Configure<TSessionBuilder>(Action<TSessionBuilder> configure, ConfigurationMode mode = default)
        where TSessionBuilder : IAtataSessionBuilder =>
        Configure(null, configure, mode);

    /// <summary>
    /// <para>
    /// Configures existing <typeparamref name="TSessionBuilder"/> session builder that has the specified <paramref name="name"/>.
    /// </para>
    /// <para>
    /// The <paramref name="mode"/> (<see cref="ConfigurationMode.ConfigureOrThrow"/> by default)
    /// parameter specifies the behavior of the fallback logic when the session builder is not found:
    /// <list type="bullet">
    /// <item><see cref="ConfigurationMode.ConfigureOrThrow"/> - configures the builder or throws the <see cref="AtataSessionBuilderNotFoundException"/> if it is not found.</item>
    /// <item><see cref="ConfigurationMode.ConfigureIfExists"/> - configures the builder only if it exists; otherwise, no action is taken.</item>
    /// <item><see cref="ConfigurationMode.ConfigureOrAdd"/> - configures the builder if it exists, or adds a new builder if it does not exist.</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder.</typeparam>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TSessionBuilder"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Configure<TSessionBuilder>(string? name, Action<TSessionBuilder> configure, ConfigurationMode mode = default)
        where TSessionBuilder : IAtataSessionBuilder
    {
        Guard.ThrowIfNull(configure);

        switch (mode)
        {
            case ConfigurationMode.ConfigureOrThrow:
                ConfigureOrThrow(name, configure);
                break;
            case ConfigurationMode.ConfigureIfExists:
                ConfigureIfExists(name, configure);
                break;
            case ConfigurationMode.ConfigureOrAdd:
                ConfigureOrAdd(name, configure);
                break;
            default:
                throw Guard.CreateArgumentExceptionForUnsupportedValue(mode);
        }

        return _atataContextBuilder;
    }

    /// <summary>
    /// Configures existing session builder that has the specified <paramref name="sessionType"/> and <paramref name="name"/>.
    /// The <paramref name="mode"/> (<see cref="ConfigurationMode.ConfigureOrThrow"/> by default)
    /// parameter specifies the behavior of the fallback logic when the session builder is not found:
    /// <list type="bullet">
    /// <item><see cref="ConfigurationMode.ConfigureOrThrow"/> - configures the builder or throws the <see cref="AtataSessionBuilderNotFoundException"/> if it is not found.</item>
    /// <item><see cref="ConfigurationMode.ConfigureIfExists"/> - configures the builder only if it exists; otherwise, no action is taken.</item>
    /// <item><see cref="ConfigurationMode.ConfigureOrAdd"/> - configures the builder if it exists, or adds a new builder if it does not exist.</item>
    /// </list>
    /// </summary>
    /// <param name="sessionType">The type of the session.</param>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="IAtataSessionBuilder"/>.</param>
    /// <param name="mode">The configuration mode, which is <see cref="ConfigurationMode.ConfigureOrThrow"/> by default.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Configure(Type sessionType, string? name, Action<IAtataSessionBuilder> configure, ConfigurationMode mode = default)
    {
        Guard.ThrowIfNull(sessionType);
        Guard.ThrowIfNull(configure);

        switch (mode)
        {
            case ConfigurationMode.ConfigureOrThrow:
                ConfigureOrThrow(sessionType, name, configure);
                break;
            case ConfigurationMode.ConfigureIfExists:
                ConfigureIfExists(sessionType, name, configure);
                break;
            case ConfigurationMode.ConfigureOrAdd:
                ConfigureOrAdd(sessionType, name, configure);
                break;
            default:
                throw Guard.CreateArgumentExceptionForUnsupportedValue(mode);
        }

        return _atataContextBuilder;
    }

    private void ConfigureOrThrow<TSessionBuilder>(string? name, Action<TSessionBuilder> configure)
        where TSessionBuilder : IAtataSessionBuilder
    {
        var sessionBuilder = GetSessionProviderOrNull<TSessionBuilder>(name)
            ?? throw AtataSessionBuilderNotFoundException.ByBuilderType(typeof(TSessionBuilder), name);

        configure.Invoke(sessionBuilder);
    }

    private void ConfigureOrThrow(Type sessionType, string? name, Action<IAtataSessionBuilder> configure)
    {
        var sessionBuilder = GetSessionBuilderOrNull(sessionType, name)
            ?? throw AtataSessionBuilderNotFoundException.BySessionType(sessionType, name);

        configure.Invoke(sessionBuilder);
    }

    private void ConfigureIfExists<TSessionBuilder>(string? name, Action<TSessionBuilder> configure)
        where TSessionBuilder : IAtataSessionBuilder
    {
        var sessionBuilder = GetSessionProviderOrNull<TSessionBuilder>(name);

        if (sessionBuilder is not null)
            configure.Invoke(sessionBuilder);
    }

    private void ConfigureIfExists(Type sessionType, string? name, Action<IAtataSessionBuilder> configure)
    {
        var sessionBuilder = GetSessionBuilderOrNull(sessionType, name);

        if (sessionBuilder is not null)
            configure.Invoke(sessionBuilder);
    }

    private void ConfigureOrAdd<TSessionBuilder>(string? name, Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder
    {
        var sessionBuilder = GetSessionProviderOrNull<TSessionBuilder>(name);

        if (sessionBuilder is null)
        {
            sessionBuilder = ActivatorEx.CreateInstance<TSessionBuilder>();
            sessionBuilder.StartScopes = _defaultStartScopes;
            sessionBuilder.Name = name;

            configure?.Invoke(sessionBuilder);
            _sessionProviders.Add(sessionBuilder);
        }
        else
        {
            configure?.Invoke(sessionBuilder);
        }
    }

    private void ConfigureOrAdd(Type sessionType, string? name, Action<IAtataSessionBuilder>? configure = null)
    {
        var sessionBuilder = GetSessionBuilderOrNull(sessionType, name);

        if (sessionBuilder is null)
        {
            sessionBuilder = CreateSessionBuilderBySessionType(sessionType);
            sessionBuilder.StartScopes = _defaultStartScopes;
            sessionBuilder.Name = name;

            configure?.Invoke(sessionBuilder);
            _sessionProviders.Add(sessionBuilder);
        }
        else
        {
            configure?.Invoke(sessionBuilder);
        }
    }

    /// <summary>
    /// Creates a request to borrow a session of the specified <typeparamref name="TSession"/> type with the specified <paramref name="sessionName"/>,
    /// adds it to the session providers list.
    /// </summary>
    /// <typeparam name="TSession">The type of the session to borrow.</typeparam>
    /// <param name="sessionName">The name of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Borrow<TSession>(string? sessionName)
        where TSession : AtataSession
        =>
        Borrow<TSession>(x => x.Name = sessionName);

    /// <summary>
    /// Creates a request to borrow a session of the specified <typeparamref name="TSession"/> type,
    /// calls <paramref name="configure"/> delegate,
    /// adds it to the session providers list.
    /// </summary>
    /// <typeparam name="TSession">The type of the session to borrow.</typeparam>
    /// <param name="configure">An action delegate to configure the <see cref="AtataSessionBorrowRequestBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Borrow<TSession>(Action<AtataSessionBorrowRequestBuilder>? configure = null)
        where TSession : AtataSession
        =>
        Borrow(typeof(TSession), configure);

    /// <summary>
    /// Creates a request to borrow a session of the specified <paramref name="sessionType"/>,
    /// calls <paramref name="configure"/> delegate,
    /// adds it to the session providers list.
    /// </summary>
    /// <param name="sessionType">The type of the session to borrow.</param>
    /// <param name="configure">An action delegate to configure the <see cref="AtataSessionBorrowRequestBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Borrow(Type sessionType, Action<AtataSessionBorrowRequestBuilder>? configure = null)
    {
        Guard.ThrowIfNot<AtataSession>(sessionType);

        AtataSessionBorrowRequestBuilder sessionRequestBuilder = new(sessionType)
        {
            StartScopes = _defaultStartScopes
        };
        configure?.Invoke(sessionRequestBuilder);

        _sessionProviders.Add(sessionRequestBuilder);
        return _atataContextBuilder;
    }

    /// <summary>
    /// Creates a request to take a session from the pool of the specified <typeparamref name="TSession"/> type with the specified <paramref name="sessionName"/>,
    /// adds it to the session providers list.
    /// </summary>
    /// <typeparam name="TSession">The type of the session to take from the pool.</typeparam>
    /// <param name="sessionName">The name of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder TakeFromPool<TSession>(string? sessionName)
        where TSession : AtataSession
        =>
        TakeFromPool<TSession>(x => x.Name = sessionName);

    /// <summary>
    /// Creates a request to take a session from the pool of the specified <typeparamref name="TSession"/> type,
    /// calls <paramref name="configure"/> delegate,
    /// adds it to the session providers list.
    /// </summary>
    /// <typeparam name="TSession">The type of the session to take from the pool.</typeparam>
    /// <param name="configure">An action delegate to configure the <see cref="AtataSessionPoolRequestBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder TakeFromPool<TSession>(Action<AtataSessionPoolRequestBuilder>? configure = null)
        where TSession : AtataSession
        =>
        TakeFromPool(typeof(TSession), configure);

    /// <summary>
    /// Creates a request to take a session from the pool of the specified <paramref name="sessionType"/>,
    /// calls <paramref name="configure"/> delegate,
    /// adds it to the session providers list.
    /// </summary>
    /// <param name="sessionType">The type of the session to take from the pool.</param>
    /// <param name="configure">An action delegate to configure the <see cref="AtataSessionPoolRequestBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder TakeFromPool(Type sessionType, Action<AtataSessionPoolRequestBuilder>? configure = null)
    {
        Guard.ThrowIfNot<AtataSession>(sessionType);

        AtataSessionPoolRequestBuilder sessionRequestBuilder = new(sessionType)
        {
            StartScopes = _defaultStartScopes
        };
        configure?.Invoke(sessionRequestBuilder);

        _sessionProviders.Add(sessionRequestBuilder);
        return _atataContextBuilder;
    }

    /// <summary>
    /// Removes the specified session provider.
    /// </summary>
    /// <param name="sessionProvider">The session provider.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Remove(IAtataSessionProvider sessionProvider)
    {
        Guard.ThrowIfNull(sessionProvider);

        _sessionProviders.Remove(sessionProvider);
        return _atataContextBuilder;
    }

    /// <summary>
    /// Removes all session providers of the specified <typeparamref name="TSessionProvider"/> type.
    /// </summary>
    /// <typeparam name="TSessionProvider">The type of the session provider.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder RemoveAll<TSessionProvider>()
        where TSessionProvider : IAtataSessionProvider
    {
        _sessionProviders.RemoveAll(x => x is TSessionProvider);
        return _atataContextBuilder;
    }

    /// <summary>
    /// Removes all session providers of the specified <typeparamref name="TSessionProvider"/> type and <paramref name="name"/>.
    /// </summary>
    /// <typeparam name="TSessionProvider">The type of the session provider.</typeparam>
    /// <param name="name">The name of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder RemoveAll<TSessionProvider>(string? name)
        where TSessionProvider : IAtataSessionProvider
    {
        _sessionProviders.RemoveAll(x => x is TSessionProvider && x.Name == name);
        return _atataContextBuilder;
    }

    /// <summary>
    /// Removes all session providers of the specified <typeparamref name="TSession"/> regardless of name.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder RemoveAllBySessionType<TSession>() =>
        RemoveAllBySessionType(typeof(TSession));

    /// <summary>
    /// Removes all session providers of the specified <paramref name="sessionType"/> regardless of name.
    /// </summary>
    /// <param name="sessionType">The type of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder RemoveAllBySessionType(Type sessionType)
    {
        _sessionProviders.RemoveAll(x => IsProviderOfSessionType(x, sessionType));
        return _atataContextBuilder;
    }

    /// <summary>
    /// Removes all session providers of the specified <typeparamref name="TSession"/> and <paramref name="name"/>.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="name">The name of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder RemoveAllBySessionType<TSession>(string? name) =>
        RemoveAllBySessionType(typeof(TSession), name);

    /// <summary>
    /// Removes all session providers of the specified <paramref name="sessionType"/> and <paramref name="name"/>.
    /// </summary>
    /// <param name="sessionType">The type of the session.</param>
    /// <param name="name">The name of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder RemoveAllBySessionType(Type sessionType, string? name)
    {
        _sessionProviders.RemoveAll(x => x.Name == name && IsProviderOfSessionType(x, sessionType));
        return _atataContextBuilder;
    }

    /// <summary>
    /// Removes all session providers with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder RemoveAllBySessionName(string name)
    {
        _sessionProviders.RemoveAll(x => x.Name == name);
        return _atataContextBuilder;
    }

    /// <summary>
    /// Disables all session providers of the specified <typeparamref name="TSessionProvider"/> type.
    /// Sets their <see cref="IAtataSessionProvider.StartScopes"/> property to <see cref="AtataContextScopes.None"/>,
    /// so that the sessions will not automatically start for any scope.
    /// </summary>
    /// <typeparam name="TSessionProvider">The type of the session provider.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder DisableAll<TSessionProvider>()
        where TSessionProvider : IAtataSessionProvider
    {
        foreach (var provider in _sessionProviders)
            if (provider is TSessionProvider)
                provider.StartScopes = AtataContextScopes.None;

        return _atataContextBuilder;
    }

    /// <summary>
    /// Disables all session providers of the specified <typeparamref name="TSessionProvider"/> type and <paramref name="name"/>.
    /// Sets their <see cref="IAtataSessionProvider.StartScopes"/> property to <see cref="AtataContextScopes.None"/>,
    /// so that the sessions will not automatically start for any scope.
    /// </summary>
    /// <typeparam name="TSessionProvider">The type of the session provider.</typeparam>
    /// <param name="name">The name of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder DisableAll<TSessionProvider>(string? name)
        where TSessionProvider : IAtataSessionProvider
    {
        foreach (var provider in _sessionProviders)
            if (provider is TSessionProvider && provider.Name == name)
                provider.StartScopes = AtataContextScopes.None;

        return _atataContextBuilder;
    }

    /// <summary>
    /// Disables all session providers of the specified <typeparamref name="TSession"/> regardless of name.
    /// Sets their <see cref="IAtataSessionProvider.StartScopes"/> property to <see cref="AtataContextScopes.None"/>,
    /// so that the sessions will not automatically start for any scope.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder DisableAllBySessionType<TSession>() =>
        DisableAllBySessionType(typeof(TSession));

    /// <summary>
    /// Disables all session providers of the specified <paramref name="sessionType"/> regardless of name.
    /// Sets their <see cref="IAtataSessionProvider.StartScopes"/> property to <see cref="AtataContextScopes.None"/>,
    /// so that the sessions will not automatically start for any scope.
    /// </summary>
    /// <param name="sessionType">The type of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder DisableAllBySessionType(Type sessionType)
    {
        foreach (var provider in _sessionProviders)
            if (IsProviderOfSessionType(provider, sessionType))
                provider.StartScopes = AtataContextScopes.None;

        return _atataContextBuilder;
    }

    /// <summary>
    /// Disables all session providers of the specified <typeparamref name="TSession"/> and <paramref name="name"/>.
    /// Sets their <see cref="IAtataSessionProvider.StartScopes"/> property to <see cref="AtataContextScopes.None"/>,
    /// so that the sessions will not automatically start for any scope.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="name">The name of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder DisableAllBySessionType<TSession>(string? name) =>
        DisableAllBySessionType(typeof(TSession), name);

    /// <summary>
    /// Disables all session providers of the specified <paramref name="sessionType"/> and <paramref name="name"/>.
    /// Sets their <see cref="IAtataSessionProvider.StartScopes"/> property to <see cref="AtataContextScopes.None"/>,
    /// so that the sessions will not automatically start for any scope.
    /// </summary>
    /// <param name="sessionType">The type of the session.</param>
    /// <param name="name">The name of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder DisableAllBySessionType(Type sessionType, string? name)
    {
        foreach (var provider in _sessionProviders)
            if (provider.Name == name && IsProviderOfSessionType(provider, sessionType))
                provider.StartScopes = AtataContextScopes.None;

        return _atataContextBuilder;
    }

    /// <summary>
    /// Disables all session providers with the specified <paramref name="name"/>.
    /// Sets their <see cref="IAtataSessionProvider.StartScopes"/> property to <see cref="AtataContextScopes.None"/>,
    /// so that the sessions will not automatically start for any scope.
    /// </summary>
    /// <param name="name">The name of the session.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder DisableAllBySessionName(string name)
    {
        foreach (var provider in _sessionProviders)
            if (provider.Name == name)
                provider.StartScopes = AtataContextScopes.None;

        return _atataContextBuilder;
    }

    /// <summary>
    /// Clears all session providers.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Clear()
    {
        _sessionProviders.Clear();
        return _atataContextBuilder;
    }

    private static IAtataSessionBuilder CreateSessionBuilderBySessionType(Type sessionType)
    {
        MethodInfo factoryMethod = sessionType.GetMethodWithThrowOnError(
            "CreateBuilder",
            BindingFlags.Public | BindingFlags.Static,
            Type.EmptyTypes);

        return (IAtataSessionBuilder)factoryMethod.InvokeWithExceptionUnwrapping(null)!;
    }

    private static bool DoesSessionStartScopeSatisfyContextScope(AtataContextScopes? sessionStartScopes, AtataContextScope? scope) =>
        scope switch
        {
            AtataContextScope.Test => sessionStartScopes is null || sessionStartScopes.Value.HasFlag(AtataContextScopes.Test),
            AtataContextScope.TestSuite => sessionStartScopes is null || sessionStartScopes.Value.HasFlag(AtataContextScopes.TestSuite),
            AtataContextScope.TestSuiteGroup => sessionStartScopes is null || sessionStartScopes.Value.HasFlag(AtataContextScopes.TestSuiteGroup),
            AtataContextScope.Namespace => sessionStartScopes is null || sessionStartScopes.Value.HasFlag(AtataContextScopes.Namespace),
            AtataContextScope.Global => sessionStartScopes is null || sessionStartScopes.Value.HasFlag(AtataContextScopes.Global),
            null => sessionStartScopes is null,
            _ => false
        };

    private static bool IsProviderOfSessionType(IAtataSessionProvider provider, Type sessionType) =>
        provider switch
        {
            IAtataSessionBuilder builder => IsBuilderOfSessionType(builder, sessionType),
            AtataSessionBorrowRequestBuilder borrowRequest => borrowRequest.Type == sessionType,
            AtataSessionPoolRequestBuilder poolRequest => poolRequest.Type == sessionType,
            _ => false
        };

    private static bool IsBuilderOfSessionType(IAtataSessionBuilder builder, Type sessionType) =>
        sessionType.IsAssignableFrom(AtataSessionTypeMap.ResolveSessionTypeByBuilderType(builder.GetType()));

    private TSessionProvider? GetSessionProviderOrNull<TSessionProvider>(string? name)
        where TSessionProvider : IAtataSessionProvider
        =>
        _sessionProviders.OfType<TSessionProvider>()
            .LastOrDefault(x => x.Name == name);

    private IAtataSessionBuilder? GetSessionBuilderOrNull(Type sessionType, string? name) =>
        _sessionProviders.OfType<IAtataSessionBuilder>()
            .LastOrDefault(x => x.Name == name && IsBuilderOfSessionType(x, sessionType));
}
