namespace Atata.UnitTests.DataProvision;

public static class StaticSubjectTests
{
    public static class ResultOf
    {
        [Test]
        public static void ProviderName() =>
            Subject.ResultOf(() => TestClass.GetEntity(10))
                .ProviderName.Should().Be("StaticSubjectTests.TestClass.GetEntity(10) => result");

        [Test]
        public static void Function_Returns() =>
            Subject.ResultOf(() => TestClass.GetEntity(10))
                .ValueOf(x => x.Id).Should.Be(10)
                .ValueOf(x => x.Name).Should.BeNull();

        [Test]
        public static void Function_Throws()
        {
            var result = Subject.ResultOf(() => TestClass.GetEntity(null!));

            Assert.Throws<ArgumentNullException>(() =>
                _ = result.Object);
        }

        [Test]
        public static void ValueTaskFunction_Returns() =>
            Subject.ResultOf(() => TestClass.GetEntityAsValueTaskAsync("x"))
                .ValueOf(x => x.Name).Should.Be("x");

        [Test]
        public static void ValueTaskFunction_Throws()
        {
            var result = Subject.ResultOf(() => TestClass.GetEntityAsValueTaskAsync(null!));

            Assert.Throws<ArgumentNullException>(() =>
                _ = result.Object);
        }

        [Test]
        public static void TaskFunction_Returns() =>
            Subject.ResultOf(() => TestClass.GetEntityAsTaskAsync("x"))
                .ValueOf(x => x.Name).Should.Be("x");

        [Test]
        public static void TaskFunction_Throws()
        {
            var result = Subject.ResultOf(() => TestClass.GetEntityAsTaskAsync(null!));

            Assert.Throws<ArgumentNullException>(() =>
                _ = result.Object);
        }
    }

    public static class SubjectOf
    {
        [Test]
        public static void ProviderName() =>
            Subject.SubjectOf(() => TestClass.GetEntity(10))
                .ProviderName.Should().Be("StaticSubjectTests.TestClass.GetEntity(10)");
    }

    public static class Invoking
    {
        [Test]
        public static void ProviderName() =>
            Subject.Invoking(() => TestClass.GetEntity(10))
                .ProviderName.Should().Be("StaticSubjectTests.TestClass.GetEntity(10)");

        [Test]
        public static void Function_Should_Throw() =>
            Subject.Invoking(() => TestClass.GetEntity(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public static void Function_Should_Not_Throw() =>
            Subject.Invoking(() => TestClass.GetEntity(10))
                .Should.Not.Throw();

        [Test]
        public static void ValueTaskFunction_Should_Throw() =>
            Subject.Invoking(() => TestClass.ThrowAsValueTaskAsync())
                .Should.Throw<InvalidOperationException>();

        [Test]
        public static void ValueTaskFunction_Should_Not_Throw() =>
            Subject.Invoking(() => TestClass.DoAsValueTaskAsync())
                .Should.Not.Throw();

        [Test]
        public static void ResultValueTaskFunction_Should_Throw() =>
            Subject.Invoking(() => TestClass.GetEntityAsValueTaskAsync(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public static void ResultValueTaskFunction_Should_Not_Throw() =>
            Subject.Invoking(() => TestClass.GetEntityAsValueTaskAsync("x"))
                .Should.Not.Throw();

        [Test]
        public static void TaskFunction_Should_Throw() =>
            Subject.Invoking(() => TestClass.ThrowAsTaskAsync())
                .Should.Throw<InvalidOperationException>();

        [Test]
        public static void TaskFunction_Should_Not_Throw() =>
            Subject.Invoking(() => TestClass.DoAsTaskAsync())
                .Should.Not.Throw();

        [Test]
        public static void ResultTaskFunction_Should_Throw() =>
            Subject.Invoking(() => TestClass.GetEntityAsTaskAsync(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public static void ResultTaskFunction_Should_Not_Throw() =>
            Subject.Invoking(() => TestClass.GetEntityAsTaskAsync("x"))
                .Should.Not.Throw();
    }

    public static class DynamicInvoking
    {
        [Test]
        public static void ProviderName() =>
            Subject.DynamicInvoking(() => TestClass.GetEntity(10))
                .ProviderName.Should().Be("StaticSubjectTests.TestClass.GetEntity(10)");

        [Test]
        public static void Function_Should_Throw() =>
            Subject.DynamicInvoking(() => TestClass.GetEntity(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public static void Function_Should_Not_Throw() =>
            Subject.DynamicInvoking(() => TestClass.GetEntity(10))
                .Should.Not.Throw();

        [Test]
        public static void ValueTaskFunction_Should_Throw() =>
            Subject.DynamicInvoking(() => TestClass.ThrowAsValueTaskAsync())
                .Should.Throw<InvalidOperationException>();

        [Test]
        public static void ValueTaskFunction_Should_Not_Throw() =>
            Subject.DynamicInvoking(() => TestClass.DoAsValueTaskAsync())
                .Should.Not.Throw();

        [Test]
        public static void ResultValueTaskFunction_Should_Throw() =>
            Subject.DynamicInvoking(() => TestClass.GetEntityAsValueTaskAsync(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public static void ResultValueTaskFunction_Should_Not_Throw() =>
            Subject.DynamicInvoking(() => TestClass.GetEntityAsValueTaskAsync("x"))
                .Should.Not.Throw();

        [Test]
        public static void TaskFunction_Should_Throw() =>
            Subject.DynamicInvoking(() => TestClass.ThrowAsTaskAsync())
                .Should.Throw<InvalidOperationException>();

        [Test]
        public static void TaskFunction_Should_Not_Throw() =>
            Subject.DynamicInvoking(() => TestClass.DoAsTaskAsync())
                .Should.Not.Throw();

        [Test]
        public static void ResultTaskFunction_Should_Throw() =>
            Subject.DynamicInvoking(() => TestClass.GetEntityAsTaskAsync(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public static void ResultTaskFunction_Should_Not_Throw() =>
            Subject.DynamicInvoking(() => TestClass.GetEntityAsTaskAsync("x"))
                .Should.Not.Throw();
    }

    public static class TestClass
    {
        public static TestEntity GetEntity(int id) =>
            new() { Id = id };

        public static TestEntity GetEntity(string name)
        {
            ArgumentNullException.ThrowIfNull(name);

            return new() { Name = name };
        }

        public static async ValueTask ThrowAsValueTaskAsync()
        {
            await Task.Delay(1);
            throw new InvalidOperationException();
        }

        public static async ValueTask DoAsValueTaskAsync() =>
            await Task.Delay(1);

        public static async ValueTask<TestEntity> GetEntityAsValueTaskAsync(string name)
        {
            ArgumentNullException.ThrowIfNull(name);

            await Task.Delay(1);
            return new() { Name = name };
        }

        public static async Task ThrowAsTaskAsync()
        {
            await Task.Delay(1);
            throw new InvalidOperationException();
        }

        public static async Task DoAsTaskAsync() =>
            await Task.Delay(1);

        public static async Task<TestEntity> GetEntityAsTaskAsync(string name)
        {
            ArgumentNullException.ThrowIfNull(name);

            await Task.Delay(1);
            return new() { Name = name };
        }
    }

    public class TestEntity
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
