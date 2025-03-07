#nullable enable

namespace Atata;

public static class WebSessionAtataContextExtensions
{
    public static WebSession GetWebSession(this AtataContext atataContext) =>
        atataContext.Sessions.Get<WebSession>();
}
