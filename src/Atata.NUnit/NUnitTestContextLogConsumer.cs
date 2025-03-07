namespace Atata.NUnit;

public class NUnitTestContextLogConsumer : TextOutputLogConsumer, IInitializableLogConsumer
{
    private TestExecutionContext _testExecutionContext = null!;

    public void Initialize() =>
        _testExecutionContext = TestExecutionContext.CurrentContext;

    protected override void Write(string completeMessage)
    {
        if (TestExecutionContext.CurrentContext == _testExecutionContext)
            _testExecutionContext.OutWriter.WriteLine(completeMessage);
    }

    object ICloneable.Clone() =>
        new NUnitTestContextLogConsumer();
}
