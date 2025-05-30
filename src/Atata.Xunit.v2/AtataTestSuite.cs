namespace Atata.Xunit;

/// <summary>
/// Represents a base class for Atata Xunit test suite/class.
/// providing configuration and initialization for the test <see cref="AtataContext"/>.
/// </summary>
public abstract class AtataTestSuite : AtataFixture
{
    private readonly ITestOutputHelper _output;

    /// <summary>
    /// Initializes a new instance of the <see cref="AtataTestSuite"/> class
    /// with the <see cref="AtataContextScope.Test"/> context scope.
    /// </summary>
    /// <param name="output">The test output helper.</param>
    protected AtataTestSuite(ITestOutputHelper output)
        : base(AtataContextScope.Test) =>
        _output = output;

    private protected sealed override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        string testFullName = ResolveTestName(_output);
        Type testClassType = GetType();
        string testName = testFullName.Replace(testClassType.FullName!, null).TrimStart('.');

        builder.UseTestName(testName);
        builder.UseTestSuiteType(testClassType);

        if (CollectionResolver.TryResolveCollectionName(testClassType, out var collectionName))
            builder.UseTestSuiteGroupName(collectionName);

        builder.LogConsumers.Add(new TextOutputLogConsumer(_output.WriteLine));

        ConfigureTestAtataContext(builder);
    }

    /// <summary>
    /// Configures the test <see cref="AtataContext"/>.
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="builder">The <see cref="AtataContextBuilder"/> used to configure the context.</param>
    protected virtual void ConfigureTestAtataContext(AtataContextBuilder builder)
    {
    }

    protected void Execute(Action action)
    {
        try
        {
            action?.Invoke();
        }
        catch (Exception exception)
        {
            Context.HandleTestResultException(exception);
            throw;
        }
    }

    private static string ResolveTestName(ITestOutputHelper output)
    {
        FieldInfo[] outputTypeFields = output.GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        Array.Find(outputTypeFields, x => x.FieldType == typeof(ITest));
        ITest test = (ITest)Array.Find(outputTypeFields, x => x.FieldType == typeof(ITest))
            !.GetValue(output)!;

        return test.DisplayName;
    }
}
