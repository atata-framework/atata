namespace Atata.AspNetCore;

public class WebApplicationSessionBuilder<TSession> : WebApplicationSessionBuilder<TSession, WebApplicationSessionBuilder<TSession>>
    where TSession : WebApplicationSession, new()
{
}
