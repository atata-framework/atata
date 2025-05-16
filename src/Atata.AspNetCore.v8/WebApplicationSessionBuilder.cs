namespace Atata.AspNetCore;

/// <summary>
/// Provides a builder for creating and configuring a <see cref="WebApplicationSession"/>.
/// </summary>
/// <remarks>
/// This class serves as a non-generic implementation of <see cref="WebApplicationSessionBuilder{TSession, TBuilder}"/>
/// for the <see cref="WebApplicationSession"/> type.
/// </remarks>
public class WebApplicationSessionBuilder : WebApplicationSessionBuilder<WebApplicationSession, WebApplicationSessionBuilder>
{
}
