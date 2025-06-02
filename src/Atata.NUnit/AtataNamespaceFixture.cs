namespace Atata.NUnit;

/// <summary>
/// Represents a base class for namespace-level setup and tear-down of <see cref="AtataContext"/> in NUnit tests.
/// </summary>
[SetUpFixture]
public abstract class AtataNamespaceFixture
{
    protected AtataContext Context { get; private set; } = null!;

    /// <summary>
    /// Sets up the <see cref="AtataContext"/> for the namespace.
    /// The method is executed once before any tests in the namespace are run.
    /// </summary>
    [OneTimeSetUp]
    public void SetUpNamespaceAtataContext()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Namespace)
            .UseDefaultCancellationToken(TestExecutionContext.CurrentContext.CancellationToken);

        ConfigureNamespaceAtataContext(builder);

        Context = builder.Build();
    }

    /// <summary>
    /// Tears down the <see cref="AtataContext"/> for the namespace.
    /// The method is executed once after all tests in the namespace have run.
    /// </summary>
    [OneTimeTearDown]
    public void TearDownNamespaceAtataContext() =>
        NUnitAtataContextCompletionHandler.Complete(Context);

    /// <summary>
    /// Configures the namespace <see cref="AtataContext"/>.
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="AtataContextBuilder"/> used to configure the context.</param>
    protected virtual void ConfigureNamespaceAtataContext(AtataContextBuilder builder)
    {
    }
}
