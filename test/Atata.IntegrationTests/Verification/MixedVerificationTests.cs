﻿namespace Atata.IntegrationTests.Verification;

[Explicit("The tests are intended to fail and their results can only be validated manually.")]
public abstract class MixedVerificationTests
{
    private readonly Subject<string> _sut = "abc".ToSutSubject();

    private readonly Subject<ThrowingSut> _throwingSut = new ThrowingSut().ToSutSubject();

    protected AtataContext Context { get; private set; }

    [SetUp]
    public void SetUpTest() =>
        Context = BuildAtataContext();

    [TearDown]
    public void TearDown() =>
        Context?.Dispose();

    [Test]
    public void Fail() =>
        _sut.Should.Contain('x');

    [Test]
    public void TwoWarnings()
    {
        _sut.ExpectTo.Contain('x');
        _sut.ExpectTo.Contain('y');
    }

    [Test]
    public void TwoWarningsThenFailedAssertion()
    {
        _sut.ExpectTo.Contain('x');
        _sut.ExpectTo.Contain('y');
        _sut.Should.Contain('z');
    }

    [Test]
    public void TwoWarningsThenThrowInAssertion()
    {
        _sut.ExpectTo.Contain('x');
        _sut.ExpectTo.Contain('y');
        _throwingSut.ValueOf(x => x.ThrowingProperty).Should.Contain('z');
    }

    [Test]
    public void TwoWarningsThenThrow()
    {
        _sut.ExpectTo.Contain('x');
        _sut.ExpectTo.Contain('y');
        throw new InvalidOperationException("Expect me to fail.");
    }

    [Test]
    public void WithinAggregateAssert_TwoFailedAssertions() =>
        _sut.AggregateAssert(
            x =>
            {
                x.Should.Contain('z');
                x.Should.Contain('w');
            },
            "aggr");

    [Test]
    public void WithinAggregateAssert_TwoWarningsThenTwoFailedAssertions() =>
        _sut.ExpectTo.Contain('x')
            .AggregateAssert(
                x =>
                {
                    x.ExpectTo.Contain('y');
                    x.Should.Contain('z');
                    x.Should.Contain('w');
                },
                "aggr");

    [Test]
    public void WithinAggregateAssert_TwoWarningsThenThrowInAssertion() =>
        _sut.ExpectTo.Contain('x')
            .AggregateAssert(
                x =>
                {
                    x.ExpectTo.Contain('y');
                    _throwingSut.ValueOf(x => x.ThrowingProperty).Should.Contain('z');
                },
                "<aggregate section>");

    [Test]
    public void WithinAggregateAssert_TwoWarningsThenThrow() =>
        _sut.ExpectTo.Contain('x')
            .AggregateAssert(
                x =>
                {
                    x.ExpectTo.Contain('y');
                    throw new InvalidOperationException("Expect me to fail.");
                },
                "<aggregate section>");

    [Test]
    public void Throw() =>
        throw new InvalidOperationException(
            "Expect me to fail.",
            new InvalidDataException("<inner exception message>"));

    [Test]
    public void WithinStep_Throw() =>
        AtataContext.Current.Report.Step(
            "Step #1",
            _ =>
            throw new InvalidOperationException(
                "Expect me to fail.",
                new InvalidDataException("<inner exception message>")));

    [Test]
    public void WithinStep_Fail() =>
        AtataContext.Current.Report.Step(
            "Step #1",
            _ => _sut.Should.Contain('x'));

    [Test]
    public void WithinStep_WithinAggregateAssert_TwoWarningsThenThrowInAssertion() =>
        AtataContext.Current.Report.Step(
            "Step #1",
            _ =>
            _sut.AggregateAssert(
                x =>
                {
                    x.ExpectTo.Contain('x');
                    x.Should.Contain('y');
                },
                "<aggregate section>"));

    [Test]
    public void ThrowInAssertion() =>
        _throwingSut.ValueOf(x => x.ThrowingProperty).Should.Contain('z');

    protected abstract AtataContext BuildAtataContext();

    public sealed class ThrowingSut
    {
        public string ThrowingProperty =>
            throw new InvalidOperationException(
                "Expect me to fail.",
                new InvalidDataException("<inner exception message>"));
    }

    public class Native : MixedVerificationTests
    {
        protected override AtataContext BuildAtataContext()
        {
            var builder = AtataContext.CreateBuilder(AtataContextScope.Test);

            builder.LogConsumers.Add(new TextOutputLogConsumer(TestContext.WriteLine));

            return builder.Build();
        }
    }

    public sealed class NativeWithErrorHandling : Native
    {
        [TearDown]
        public void TearDownTest()
        {
            if (Context is not null && TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                Context.HandleTestResultException(
                    TestContext.CurrentContext.Result.Message,
                    TestContext.CurrentContext.Result.StackTrace);
            }
        }
    }

    // Commented temporarily due to UseAllNUnitFeatures method migration to Atata.NUnit.
    ////public sealed class NUnit : MixedVerificationTests
    ////{
    ////    protected override AtataContext BuildAtataContext() =>
    ////        AtataContext.CreateBuilder(AtataContextScope.Test)
    ////            .UseAllNUnitFeatures()
    ////            .Build();
    ////}
}
