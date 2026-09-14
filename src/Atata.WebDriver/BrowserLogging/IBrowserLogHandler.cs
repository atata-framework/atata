namespace Atata.WebDriver;

internal interface IBrowserLogHandler
{
    void Handle(BrowserLogEntry entry);
}
