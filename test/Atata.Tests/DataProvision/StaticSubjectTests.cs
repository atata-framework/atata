using System;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests.DataProvision
{
    [TestFixture]
    public static class StaticSubjectTests
    {
        [TestFixture]
        public static class ResultOf
        {
            [Test]
            public static void ProviderName() =>
                Subject.ResultOf(() => TestClass.GetEntity(10))
                    .ProviderName.Should().Be("StaticSubjectTests.TestClass.GetEntity(10) => result");

            [Test]
            public static void Returns() =>
                Subject.ResultOf(() => TestClass.GetEntity(10))
                    .ValueOf(x => x.Id).Should.Equal(10)
                    .ValueOf(x => x.Name).Should.BeNull();

            [Test]
            public static void Throws()
            {
                var result = Subject.ResultOf(() => TestClass.GetEntity(null));

                Assert.Throws<ArgumentNullException>(() =>
                    _ = result.Object);
            }
        }

        [TestFixture]
        public static class SubjectOf
        {
            [Test]
            public static void ProviderName() =>
                Subject.SubjectOf(() => TestClass.GetEntity(10))
                    .ProviderName.Should().Be("StaticSubjectTests.TestClass.GetEntity(10)");
        }

        [TestFixture]
        public static class Invoking
        {
            [Test]
            public static void ProviderName() =>
                Subject.Invoking(() => TestClass.GetEntity(10))
                    .ProviderName.Should().Be("StaticSubjectTests.TestClass.GetEntity(10)");

            [Test]
            public static void Function_Should_Throw() =>
                Subject.Invoking(() => TestClass.GetEntity(null))
                    .Should.Throw<ArgumentNullException>();

            [Test]
            public static void Function_Should_Not_Throw() =>
                Subject.Invoking(() => TestClass.GetEntity(10))
                    .Should.Not.Throw();
        }

        public static class TestClass
        {
            public static TestEntity GetEntity(int id) =>
                new() { Id = id };

            public static TestEntity GetEntity(string name)
            {
                name.CheckNotNull(nameof(name));
                return new TestEntity { Name = name };
            }
        }

        public class TestEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
