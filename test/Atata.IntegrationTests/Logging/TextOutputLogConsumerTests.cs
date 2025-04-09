namespace Atata.IntegrationTests.Logging;

public sealed class TextOutputLogConsumerTests : TestSuiteBase
{
    private List<string> _logLines;

    [SetUp]
    public void SetUp()
    {
        _logLines = [];

        var builder = ConfigureSessionlessAtataContext();
        builder.LogConsumers.Add(new TextOutputLogConsumer(_logLines.Add));
        builder.Build();
    }

    [Test]
    public void Log_WithExternalSource()
    {
        CurrentContext.Log.ForExternalSource("Ext").Trace("Text");

        _logLines[^1].Should().EndWith("TRACE {Ext} Text");
    }

    [Test]
    public void Log_WithCategory()
    {
        CurrentContext.Log.ForCategory("Cat").Trace("Text");

        _logLines[^1].Should().EndWith("TRACE [Cat] Text");
    }

    [Test]
    public void Log_WithExternalSourceAndCategory()
    {
        CurrentContext.Log.ForExternalSource("Ext").ForCategory("Cat").Trace("Text");

        _logLines[^1].Should().EndWith("TRACE {Ext} [Cat] Text");
    }

    [Test]
    public void Log_WithCategoryAndExternalSource()
    {
        CurrentContext.Log.ForCategory("Cat").ForExternalSource("Ext").Trace("Text");

        _logLines[^1].Should().EndWith("TRACE {Ext} [Cat] Text");
    }
}
