using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Atata.UnitTests.DataProvision;

public static class ObjectVerificationProviderExtensionMethodTests
{
    public class Satisfy_Expression : ExtensionMethodTestFixture<string, Satisfy_Expression>
    {
        static Satisfy_Expression() =>
            For("abc123")
                .ThrowsArgumentNullException(x => x.Satisfy(null))
                .Pass(x => x.Satisfy(x => x.Contains("abc") && x.Contains("123")))
                .Fail(x => x.Satisfy(x => x == "xyz"));
    }

    public class Satisfy_Function : ExtensionMethodTestFixture<int, Satisfy_Function>
    {
        static Satisfy_Function() =>
            For(5)
                .ThrowsArgumentNullException(x => x.Satisfy(null, "..."))
                .Pass(x => x.Satisfy(x => x > 1 && x < 10, "..."))
                .Fail(x => x.Satisfy(x => x == 7, "..."));
    }

    public class Satisfy_IEnumerable_Expression : ExtensionMethodTestFixture<Subject<string>[], Satisfy_IEnumerable_Expression>
    {
        static Satisfy_IEnumerable_Expression() =>
            For(new[] { "a".ToSubject(), "b".ToSubject(), "c".ToSubject() })
                .ThrowsArgumentNullException(x => x.Satisfy(null as Expression<Func<IEnumerable<string>, bool>>))
                .Pass(x => x.Satisfy(x => x.Contains("a") && x.Contains("c")))
                .Fail(x => x.Satisfy((IEnumerable<string> x) => x.Any(y => y.Contains('z'))));
    }

    public class BeEquivalent : ExtensionMethodTestFixture<int[], BeEquivalent>
    {
        static BeEquivalent() =>
            For(new[] { 1, 1, 2, 3, 5 })
                .ThrowsArgumentNullException(x => x.BeEquivalent(null))
                .Pass(x => x.BeEquivalent(1, 1, 2, 3, 5))
                .Pass(x => x.BeEquivalent(5, 1, 2, 3, 1))
                .Fail(x => x.BeEquivalent())
                .Fail(x => x.BeEquivalent(1, 2, 3, 4, 5))
                .Fail(x => x.BeEquivalent(1, 1, 2, 3));
    }

    public class BeEquivalent_WhenEmpty : ExtensionMethodTestFixture<int[], BeEquivalent_WhenEmpty>
    {
        static BeEquivalent_WhenEmpty() =>
            For(new int[0])
                .Pass(x => x.BeEquivalent())
                .Fail(x => x.BeEquivalent(1));
    }

    public class Contain_IEnumerable : ExtensionMethodTestFixture<int[], Contain_IEnumerable>
    {
        static Contain_IEnumerable() =>
            For(new[] { 1, 2, 3, 5 })
                .ThrowsArgumentNullException(x => x.Contain(null as IEnumerable<int>))
                .ThrowsArgumentException(x => x.Contain())
                .Pass(x => x.Contain(2, 3))
                .Pass(x => x.Contain(5))
                .Pass(x => x.Contain(5, 5))
                .Fail(x => x.Contain(4, 6));
    }

    public class ContainAny_IEnumerable : ExtensionMethodTestFixture<int[], ContainAny_IEnumerable>
    {
        static ContainAny_IEnumerable() =>
            For(new[] { 1, 2, 3, 5 })
                .ThrowsArgumentNullException(x => x.ContainAny(null as IEnumerable<int>))
                .ThrowsArgumentException(x => x.ContainAny())
                .Pass(x => x.ContainAny(4, 5))
                .Pass(x => x.ContainAny(5))
                .Pass(x => x.ContainAny(5, 5))
                .Fail(x => x.ContainAny(4, 6));
    }

    public class StartWith_IEnumerable : ExtensionMethodTestFixture<int[], StartWith_IEnumerable>
    {
        static StartWith_IEnumerable() =>
            For(new[] { 1, 2, 3, 5 })
                .ThrowsArgumentNullException(x => x.StartWith(null as IEnumerable<int>))
                .ThrowsArgumentException(x => x.StartWith())
                .Pass(x => x.StartWith(1))
                .Pass(x => x.StartWith(1, 2, 3))
                .Fail(x => x.StartWith(1, 2, 3, 5, 6))
                .Fail(x => x.StartWith(1, 3))
                .Fail(x => x.StartWith(9));
    }

    public class StartWithAny_IEnumerable : ExtensionMethodTestFixture<int[], StartWithAny_IEnumerable>
    {
        static StartWithAny_IEnumerable() =>
            For(new[] { 1, 2, 3, 5 })
                .ThrowsArgumentNullException(x => x.StartWithAny(null as IEnumerable<int>))
                .ThrowsArgumentException(x => x.StartWithAny())
                .Pass(x => x.StartWithAny(1))
                .Pass(x => x.StartWithAny(8, 1, 9))
                .Fail(x => x.StartWithAny(2, 3))
                .Fail(x => x.StartWithAny(9));
    }

    public class StartWithAny_string : ExtensionMethodTestFixture<string, StartWithAny_string>
    {
        static StartWithAny_string() =>
            For("abcdef")
                .ThrowsArgumentNullException(x => x.StartWithAny(null))
                .ThrowsArgumentException(x => x.StartWithAny())
                .Pass(x => x.StartWithAny("a"))
                .Pass(x => x.StartWithAny("abc"))
                .Fail(x => x.StartWithAny("zbc"))
                .Fail(x => x.StartWithAny("z", "x"));
    }

    public class EndWith_IEnumerable : ExtensionMethodTestFixture<int[], EndWith_IEnumerable>
    {
        static EndWith_IEnumerable() =>
            For(new[] { 1, 2, 3, 5 })
                .ThrowsArgumentNullException(x => x.EndWith(null as IEnumerable<int>))
                .ThrowsArgumentException(x => x.EndWith())
                .Pass(x => x.EndWith(5))
                .Pass(x => x.EndWith(2, 3, 5))
                .Fail(x => x.EndWith(0, 1, 2, 3, 5))
                .Fail(x => x.EndWith(2, 5))
                .Fail(x => x.EndWith(9));
    }

    public class EndWithAny_IEnumerable : ExtensionMethodTestFixture<int[], EndWithAny_IEnumerable>
    {
        static EndWithAny_IEnumerable() =>
            For(new[] { 1, 2, 3, 5 })
                .ThrowsArgumentNullException(x => x.EndWithAny(null as IEnumerable<int>))
                .ThrowsArgumentException(x => x.EndWithAny())
                .Pass(x => x.EndWithAny(5))
                .Pass(x => x.EndWithAny(8, 5, 9))
                .Fail(x => x.EndWithAny(2, 3))
                .Fail(x => x.EndWithAny(9));
    }

    public class EndWithAny_string : ExtensionMethodTestFixture<string, EndWithAny_string>
    {
        static EndWithAny_string() =>
            For("abcdef")
                .ThrowsArgumentNullException(x => x.EndWithAny(null))
                .ThrowsArgumentException(x => x.EndWithAny())
                .Pass(x => x.EndWithAny("f"))
                .Pass(x => x.EndWithAny("def"))
                .Fail(x => x.EndWithAny("dea"))
                .Fail(x => x.EndWithAny("a", "b"));
    }

    public abstract class ExtensionMethodTestFixture<TObject, TFixture>
        where TFixture : ExtensionMethodTestFixture<TObject, TFixture>
    {
        private static readonly TestSuiteData s_testSuiteData = new();

        private Subject<TObject> _sut;

        protected static TestSuiteBuilder For(TObject testObject)
        {
            s_testSuiteData.TestObject = testObject;
            return new TestSuiteBuilder(s_testSuiteData);
        }

        public static IEnumerable<TestCaseData> GetTestActions() =>
            GetTestActionGroups().SelectMany(x => x);

        private static IEnumerable<IEnumerable<TestCaseData>> GetTestActionGroups()
        {
            yield return GenerateTestCaseData(
                "Should passes",
                s_testSuiteData.PassFunctions,
                (sut, function) => Assert.DoesNotThrow(() => function(sut.Should)));

            yield return GenerateTestCaseData(
                "Should fails",
                s_testSuiteData.FailFunctions,
                (sut, function) => Assert.Throws<AssertionException>(() => function(sut.Should)));

            yield return GenerateTestCaseData(
                "Should.Not passes",
                s_testSuiteData.FailFunctions,
                (sut, function) => Assert.DoesNotThrow(() => function(sut.Should.Not)));

            yield return GenerateTestCaseData(
                "Should.Not fails",
                s_testSuiteData.PassFunctions,
                (sut, function) => Assert.Throws<AssertionException>(() => function(sut.Should.Not)));

            yield return GenerateTestCaseData(
                "Should throws ArgumentException",
                s_testSuiteData.ThrowingArgumentExceptionFunctions,
                (sut, function) => Assert.Throws<ArgumentException>(() => function(sut.Should)));

            yield return GenerateTestCaseData(
                "Should throws ArgumentNullException",
                s_testSuiteData.ThrowingArgumentNullExceptionFunctions,
                (sut, function) => Assert.Throws<ArgumentNullException>(() => function(sut.Should)));
        }

        private static IEnumerable<TestCaseData> GenerateTestCaseData(
            string testName,
            List<Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>>> functions,
            Action<Subject<TObject>, Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>>> assertionFunction)
        {
            RuntimeHelpers.RunClassConstructor(typeof(TFixture).TypeHandle);

            Action<Subject<TObject>> BuildTestAction(Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>> function) =>
                sut => assertionFunction(sut, function);

            return functions.Count == 1
                ? new[] { new TestCaseData(BuildTestAction(functions[0])).SetArgDisplayNames(testName) }
                : functions.Select((x, i) => new TestCaseData(BuildTestAction(x)).SetArgDisplayNames($"{testName} #{i + 1}"));
        }

        [OneTimeSetUp]
        public void SetUpFixture() =>
            _sut = s_testSuiteData.TestObject.ToSutSubject();

        [TestCaseSource(nameof(GetTestActions))]
        public void When(Action<Subject<TObject>> testAction) =>
            testAction.Invoke(_sut);

        public class TestSuiteData
        {
            public TObject TestObject { get; set; }

            public List<Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>>> PassFunctions { get; } =
                new List<Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>>>();

            public List<Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>>> FailFunctions { get; } =
                new List<Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>>>();

            public List<Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>>> ThrowingArgumentExceptionFunctions { get; } =
                new List<Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>>>();

            public List<Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>>> ThrowingArgumentNullExceptionFunctions { get; } =
                new List<Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>>>();
        }

        public class TestSuiteBuilder
        {
            private readonly TestSuiteData _context;

            public TestSuiteBuilder(TestSuiteData context) =>
                _context = context;

            public TestSuiteBuilder Pass(Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>> passFunction)
            {
                _context.PassFunctions.Add(passFunction);
                return this;
            }

            public TestSuiteBuilder Fail(Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>> failFunction)
            {
                _context.FailFunctions.Add(failFunction);
                return this;
            }

            public TestSuiteBuilder ThrowsArgumentException(Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>> throwingFunction)
            {
                _context.ThrowingArgumentExceptionFunctions.Add(throwingFunction);
                return this;
            }

            public TestSuiteBuilder ThrowsArgumentNullException(Func<IObjectVerificationProvider<TObject, Subject<TObject>>, Subject<TObject>> throwingFunction)
            {
                _context.ThrowingArgumentNullExceptionFunctions.Add(throwingFunction);
                return this;
            }
        }
    }
}
