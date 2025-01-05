#nullable enable

namespace Atata;

public class NUnitTestContextLogConsumer : TextOutputLogConsumer, IInitializableLogConsumer
{
    private static readonly MethodInfo s_writeMethod =
        Type.GetType("NUnit.Framework.TestContext,nunit.framework", true)
            .GetMethodWithThrowOnError("WriteLine", typeof(string));

    private object? _testExecutionContext;

    public void Initialize() =>
        _testExecutionContext = NUnitAdapter.GetCurrentTestExecutionContext();

    protected override void Write(string completeMessage)
    {
        if (NUnitAdapter.GetCurrentTestExecutionContext() == _testExecutionContext)
            s_writeMethod.InvokeStaticAsLambda(completeMessage);
    }

    object ICloneable.Clone() =>
        new NUnitTestContextLogConsumer();
}
