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
    /// Configures existing nameless <typeparamref name="TSessionBuilder"/> session builder;
    /// or throws <see cref="AtataSessionBuilderNotFoundException"/> if session builder of such type is not found.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Configure<TSessionBuilder>(Action<TSessionBuilder> configure)
        where TSessionBuilder : IAtataSessionBuilder =>
        Configure(null, configure);

    /// <summary>
    /// Configures existing <typeparamref name="TSessionBuilder"/> session builder that has the specified <paramref name="name"/>;
    /// or throws <see cref="AtataSessionBuilderNotFoundException"/> if session builder of such type and name is not found.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder.</typeparam>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Configure<TSessionBuilder>(string? name, Action<TSessionBuilder> configure)
        where TSessionBuilder : IAtataSessionBuilder
    {
        Guard.ThrowIfNull(configure);

        var sessionBuilder = GetSessionProviderOrNull<TSessionBuilder>(name)
            ?? throw AtataSessionBuilderNotFoundException.ByBuilderType(typeof(TSessionBuilder), name);

        configure.Invoke(sessionBuilder);

        return _atataContextBuilder;
    }

    /// <summary>
    /// Configures existing session builder that has the specified <paramref name="sessionType"/> and <paramref name="name"/>;
    /// or throws <see cref="AtataSessionBuilderNotFoundException"/> if session builder of such type and name is not found.
    /// </summary>
    /// <param name="sessionType">The type of the session.</param>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="IAtataSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Configure(Type sessionType, string? name, Action<IAtataSessionBuilder> configure)
    {
        Guard.ThrowIfNull(sessionType);
        Guard.ThrowIfNull(configure);

        var sessionBuilder = GetSessionBuilderOrNull(sessionType, name)
            ?? throw AtataSessionBuilderNotFoundException.BySessionType(sessionType, name);

        configure.Invoke(sessionBuilder);

        return _atataContextBuilder;
    }

    /// <summary>
    /// Configures nameless <typeparamref name="TSessionBuilder"/> session builder if it exists.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder ConfigureIfExists<TSessionBuilder>(Action<TSessionBuilder> configure)
        where TSessionBuilder : IAtataSessionBuilder =>
        ConfigureIfExists(null, configure);

    /// <summary>
    /// Configures <typeparamref name="TSessionBuilder"/> session builder that has the specified <paramref name="name"/> if it exists.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder.</typeparam>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder ConfigureIfExists<TSessionBuilder>(string? name, Action<TSessionBuilder> configure)
        where TSessionBuilder : IAtataSessionBuilder
    {
        Guard.ThrowIfNull(configure);

        var sessionBuilder = GetSessionProviderOrNull<TSessionBuilder>(name);

        if (sessionBuilder is not null)
            configure.Invoke(sessionBuilder);

        return _atataContextBuilder;
    }

    /// <summary>
    /// Configures session builder that has the specified <paramref name="sessionType"/> and <paramref name="name"/> if it exists.
    /// </summary>
    /// <param name="sessionType">The type of the session.</param>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="IAtataSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder ConfigureIfExists(Type sessionType, string? name, Action<IAtataSessionBuilder> configure)
    {
        Guard.ThrowIfNull(sessionType);
        Guard.ThrowIfNull(configure);

        var sessionBuilder = GetSessionBuilderOrNull(sessionType, name);

        if (sessionBuilder is not null)
            configure.Invoke(sessionBuilder);

        return _atataContextBuilder;
    }

    /// <summary>
    /// Configures existing nameless <typeparamref name="TSessionBuilder"/> session builder;
    /// or adds a new one if such builder doesn't exist.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder ConfigureOrAdd<TSessionBuilder>(Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder, new() =>
        ConfigureOrAdd(null, configure);

    /// <summary>
    /// Configures existing <typeparamref name="TSessionBuilder"/> session builder that has the specified <paramref name="name"/>;
    /// or adds a new one if such builder doesn't exist.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder.</typeparam>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder ConfigureOrAdd<TSessionBuilder>(string? name, Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder, new()
    {
        var sessionBuilder = GetSessionProviderOrNull<TSessionBuilder>(name);

        if (sessionBuilder is null)
        {
            sessionBuilder = new()
            {
                StartScopes = _defaultStartScopes,
                Name = name
            };

            configure?.Invoke(sessionBuilder);
            _sessionProviders.Add(sessionBuilder);
        }
        else
        {
            configure?.Invoke(sessionBuilder);
        }

        return _atataContextBuilder;
    }

    /// <summary>
    /// Configures existing session builder that has the specified <paramref name="sessionType"/> and <paramref name="name"/>;
    /// or adds a new one if such builder doesn't exist.
    /// </summary>
    /// <param name="sessionType">The type of the session.</param>
    /// <param name="name">The session name.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="IAtataSessionBuilder"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder ConfigureOrAdd(Type sessionType, string? name, Action<IAtataSessionBuilder>? configure = null)
    {
        Guard.ThrowIfNull(sessionType);

        var sessionBuilder = GetSessionBuilderOrNull(sessionType, name);

        if (sessionBuilder is null)
        {
            sessionBuilder = ActivatorEx.CreateInstance<IAtataSessionBuilder>(sessionType);
            sessionBuilder.StartScopes = _defaultStartScopes;
            sessionBuilder.Name = name;

            configure?.Invoke(sessionBuilder);
            _sessionProviders.Add(sessionBuilder);
        }
        else
        {
            configure?.Invoke(sessionBuilder);
        }

        return _atataContextBuilder;
    }

    public AtataContextBuilder Borrow<TSession>(string? sessionName)
        where TSession : AtataSession
        =>
        Borrow<TSession>(x => x.Name = sessionName);

    public AtataContextBuilder Borrow<TSession>(Action<AtataSessionBorrowRequestBuilder>? configure = null)
        where TSession : AtataSession
        =>
        Borrow(typeof(TSession), configure);

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

    public AtataContextBuilder TakeFromPool<TSession>(string? sessionName)
        where TSession : AtataSession
        =>
        TakeFromPool<TSession>(x => x.Name = sessionName);

    public AtataContextBuilder TakeFromPool<TSession>(Action<AtataSessionPoolRequestBuilder>? configure = null)
        where TSession : AtataSession
        =>
        TakeFromPool(typeof(TSession), configure);

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
    /// Removes a session provider by the specified <typeparamref name="TSessionProvider"/> type
    /// and <paramref name="name"/>.
    /// </summary>
    /// <typeparam name="TSessionProvider">The type of the session provider.</typeparam>
    /// <param name="name">The name of the session provider.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Remove<TSessionProvider>(string? name)
        where TSessionProvider : IAtataSessionProvider
    {
        _sessionProviders.RemoveAll(x => x is TSessionProvider && x.Name == name);
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
    /// Clears all session providers.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Clear()
    {
        _sessionProviders.Clear();
        return _atataContextBuilder;
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

    private TSessionProvider? GetSessionProviderOrNull<TSessionProvider>(string? name)
        where TSessionProvider : IAtataSessionProvider
        =>
        _sessionProviders.OfType<TSessionProvider>()
            .LastOrDefault(x => x.Name == name);

    private IAtataSessionBuilder? GetSessionBuilderOrNull(Type sessionType, string? name)
        =>
        _sessionProviders.OfType<IAtataSessionBuilder>()
            .LastOrDefault(x => x.Name == name && sessionType.IsAssignableFrom(AtataSessionTypeMap.ResolveSessionTypeByBuilderType(x.GetType())));
}
