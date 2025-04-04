#nullable enable

namespace Atata;

internal interface IBrowserLogHandler
{
    void Handle(BrowserLogEntry entry);
}
