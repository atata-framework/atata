namespace Atata.UnitTests;

public static class AtataContextParentResolverTests
{
    public sealed class FindParentContext
    {
        private AtataContext _globalContext = null!;

        [SetUp]
        public void SetUpTest() =>
            _globalContext = CreateContext(null, AtataContextScope.Global, new(null));

        [TearDown]
        public void TearDownTest() =>
            _globalContext?.Dispose();

        [Test]
        public void WithTestScope_WhenOnlyGlobalExists()
        {
            var result = Act(AtataContextScope.Test, new("TheTest", typeof(FindParentContext)));

            result.Should().Be(_globalContext);
        }

        [Test]
        public void WithTestScope_WhenTestSuiteExists()
        {
            using var testSuiteContext = CreateContext(
                _globalContext,
                AtataContextScope.TestSuite,
                new(typeof(FindParentContext)));

            var result = Act(AtataContextScope.Test, new("TheTest", typeof(FindParentContext)));

            result.Should().Be(testSuiteContext);
        }

        [Test]
        public void WithNamespaceScope_WhenParentNamespaceSuiteExists()
        {
            using var namespaceContext = CreateRootNamespaceContext();

            var result = Act(AtataContextScope.Namespace, new(typeof(FindParentContext)));

            result.Should().Be(namespaceContext);
        }

        [Test]
        public void WithTestSuiteScope_WhenParentNamespaceSuiteExists()
        {
            using var namespaceContext = CreateRootNamespaceContext();

            var result = Act(AtataContextScope.TestSuite, new(typeof(FindParentContext)));

            result.Should().Be(namespaceContext);
        }

        [Test]
        public void WithTestSuiteScope_WhenMultipleNamespaceSuitesExists()
        {
            using var rootNamespaceContext = CreateRootNamespaceContext();
            using var subNamespaceContext = CreateContext(
                rootNamespaceContext,
                AtataContextScope.Namespace,
                new(typeof(AtataContextParentResolverTests)));

            var result = Act(AtataContextScope.TestSuite, new(typeof(FindParentContext)));

            result.Should().Be(subNamespaceContext);
        }

        private static AtataContext CreateContext(AtataContext? parentContext, AtataContextScope? scope, TestInfo testInfo)
        {
            var context = new AtataContext(parentContext, scope, testInfo)
            {
                Log = Mock.Of<ILogManager>()
            };

            parentContext?.AddChildContext(context);

            return context;
        }

        private AtataContext CreateRootNamespaceContext() =>
            CreateContext(
                _globalContext,
                AtataContextScope.Namespace,
                new(typeof(AtataContext))); // Need any type from Atata namespace.

        private AtataContext? Act(AtataContextScope scope, TestInfo testInfo) =>
            AtataContextParentResolver.FindParentContext(_globalContext, scope, testInfo);
    }
}
