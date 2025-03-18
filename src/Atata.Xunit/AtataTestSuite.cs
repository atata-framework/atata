namespace Atata.Xunit;

public abstract class AtataTestSuite : AtataFixture
{
    private readonly ITestOutputHelper _output;

    protected AtataTestSuite(ITestOutputHelper output)
        : base(AtataContextScope.Test) =>
        _output = output;

    protected override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        string testFullName = ResolveTestName(_output);
        Type testClassType = GetType();
        string testName = testFullName.Replace(testClassType.FullName!, null).TrimStart('.');

        builder.UseTestName(testName);
        builder.UseTestSuiteType(testClassType);

        if (CollectionResolver.TryResolveCollectionName(testClassType, out var collectionName))
            builder.UseTestSuiteGroupName(collectionName);

        builder.LogConsumers.Add(new TextOutputLogConsumer(_output.WriteLine));
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
