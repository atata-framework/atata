using FluentAssertions;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Atata.NLog.IntegrationTests;

public abstract class TestSuiteBase
{
    [TearDown]
    public async Task TearDownTestAtataContextAsync()
    {
        var context = AtataContext.Current;

        if (context is { IsActive: true })
        {
            var testContext = TestContext.CurrentContext;

            if (testContext.Result.Outcome.Status == TestStatus.Failed)
                context.HandleTestResultException(testContext.Result.Message, testContext.Result.StackTrace);

            await context.DisposeAsync().ConfigureAwait(false);
        }
    }

    protected static AtataContextBuilder ConfigureSessionlessAtataContext()
    {
        var builder = AtataContext.CreateBuilder(AtataContextScope.Test)
            .UseCulture("en-US")
            .UseTestName(() => TestContext.CurrentContext.Test.Name)
            .UseTestSuiteName(GetCurrentTestFixtureName)
            .UseTestSuiteType(GetCurrentTestFixtureType);

        builder.LogConsumers.Add(new TextOutputLogConsumer(TestContext.WriteLine));

        return builder;
    }

    protected static void AssertThatFileExists(string filePath) =>
        Assert.That(new FileInfo(filePath), Does.Exist);

    protected static void AssertThatFileShouldContainText(string filePath, params string[] texts)
    {
        AssertThatFileExists(filePath);

        using FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using StreamReader reader = new(fileStream);

        string fileContent = reader.ReadToEnd();
        fileContent.Should().ContainAll(texts);
    }

    private static string? GetCurrentTestFixtureName()
    {
        ITest? testItem = TestExecutionContext.CurrentContext.CurrentTest;

        if (testItem is SetUpFixture)
            return testItem.TypeInfo!.Type.Name;

        while (testItem is not null)
        {
            if (testItem is TestFixture)
                return testItem.Name;

            testItem = testItem?.Parent;
        }

        return null;
    }

    private static Type? GetCurrentTestFixtureType()
    {
        ITest? testItem = TestExecutionContext.CurrentContext.CurrentTest;

        if (testItem is SetUpFixture)
            return testItem.TypeInfo!.Type;

        while (testItem is not null)
        {
            if (testItem is TestFixture)
                return testItem.TypeInfo!.Type;

            testItem = testItem.Parent;
        }

        return null;
    }
}
