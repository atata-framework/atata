namespace Atata.AspNetCore;

/// <summary>
/// Provides a builder for creating and configuring a <see cref="WebApplicationSession"/>.
/// </summary>
/// <typeparam name="TSession">The type of the session to build, which must inherit from <see cref="WebApplicationSession"/>.</typeparam>
public class WebApplicationSessionBuilder<TSession> : WebApplicationSessionBuilder<TSession, WebApplicationSessionBuilder<TSession>>
    where TSession : WebApplicationSession, new()
{
}
