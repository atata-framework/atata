namespace Atata.NUnit;

/// <summary>
/// Represents a log consumer that writes log messages to the NUnit test context output.
/// </summary>
public class NUnitTestContextLogConsumer : TextOutputLogConsumer, IInitializableLogConsumer
{
    private TestExecutionContext _testExecutionContext = null!;

    void IInitializableLogConsumer.Initialize(AtataContext context) =>
        _testExecutionContext = TestExecutionContext.CurrentContext;

    /// <summary>
    /// Writes the complete log message to the NUnit test context output.
    /// </summary>
    /// <param name="completeMessage">The complete log message to write.</param>
    protected override void Write(string completeMessage)
    {
        var currentTestExecutionContext = TestExecutionContext.CurrentContext;

        if (currentTestExecutionContext == _testExecutionContext || currentTestExecutionContext is TestExecutionContext.AdhocContext)
            _testExecutionContext.OutWriter.WriteLine(completeMessage);
    }

    object ICloneable.Clone() =>
        new NUnitTestContextLogConsumer();
}
