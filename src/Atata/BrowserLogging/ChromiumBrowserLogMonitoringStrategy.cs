using OpenQA.Selenium.DevTools;

namespace Atata;

internal sealed class ChromiumBrowserLogMonitoringStrategy : IBrowserLogMonitoringStrategy
{
    private readonly IWebDriver _driver;

    private readonly IEnumerable<IBrowserLogHandler> _browserLogHandlers;

    private DevToolsSession? _devToolsSession;

    private JavaScriptEngine? _javaScriptEngine;

    public ChromiumBrowserLogMonitoringStrategy(
        IWebDriver driver,
        IEnumerable<IBrowserLogHandler> browserLogHandlers)
    {
        _driver = driver;
        _browserLogHandlers = browserLogHandlers;
    }

    public void Start()
    {
        _devToolsSession = _driver.AsDevTools().GetDevToolsSession();
        _devToolsSession.Domains.Log.EntryAdded += OnLog;
        _devToolsSession.Domains.Log.Enable().GetAwaiter().GetResult();

        _javaScriptEngine = new(_driver);
        _javaScriptEngine.JavaScriptExceptionThrown += OnLog;
        _javaScriptEngine.JavaScriptConsoleApiCalled += OnLog;
        _javaScriptEngine.StartEventMonitoring().GetAwaiter().GetResult();
    }

    public void Stop()
    {
        if (_javaScriptEngine is not null)
        {
            _javaScriptEngine.JavaScriptExceptionThrown -= OnLog;
            _javaScriptEngine.JavaScriptConsoleApiCalled -= OnLog;
            _javaScriptEngine.StopEventMonitoring();
            _javaScriptEngine = null;
        }

        if (_devToolsSession is not null)
        {
            _devToolsSession.Domains.Log.EntryAdded -= OnLog;
            _devToolsSession.Domains.Log.Disable().GetAwaiter().GetResult();
            _devToolsSession = null;
        }

        ExtractAndHandleLogs();
    }

    private void OnLog(object sender, EventArgs e) =>
        ExtractAndHandleLogs();

    private void ExtractAndHandleLogs()
    {
        try
        {
            var logEntries = _driver.Manage().Logs.GetLog(LogType.Browser)
                .Select(BrowserLogEntry.Create);

            foreach (var logEntry in logEntries)
                foreach (var browserLogHandler in _browserLogHandlers)
                    browserLogHandler.Handle(logEntry);
        }
        catch
        {
            // Do nothing.
        }
    }
}
