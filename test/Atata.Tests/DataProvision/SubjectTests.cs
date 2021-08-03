using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests.DataProvision
{
    [TestFixture]
    public static class SubjectTests
    {
        private static Subject<Dictionary<string, int>> CreateDictionarySubject() =>
            new Subject<Dictionary<string, int>>(
                new Dictionary<string, int>
                {
                    ["a"] = 1,
                    ["b"] = 2,
                    ["c"] = 3
                });

        [Test]
        public static void Ctor_WithNullSource() =>
            new Subject<Uri>(null as Uri)
                .Value.Should().BeNull();

        [Test]
        public static void Ctor_WithObjectSourceContainingNull() =>
            new Subject<Uri>(new StaticObjectSource<Uri>(null))
                .Value.Should().BeNull();

        [Test]
        public static void Ctor_WithNullObjectSource_Throws() =>
            Assert.Throws<ArgumentNullException>(() =>
               new Subject<Uri>(null as StaticObjectSource<Uri>));

        [Test]
        public static void Owner()
        {
            var subject = new Subject<int>(42);

            ((IDataProvider<int, Subject<int>>)subject).Owner.Should().Be(subject);
        }

        [TestFixture]
        public static class ProviderName
        {
            [Test]
            public static void Default_WhenCtorWithoutProviderName() =>
                new Subject<int>(42)
                    .ProviderName.Should().Be("subject");

            [Test]
            public static void WhenCtorWithProviderName() =>
                new Subject<int>(42, "some name")
                    .ProviderName.Should().Be("some name");
        }

        [TestFixture]
        public class ValueOf
        {
            private Subject<Dictionary<string, int>> _subject;

            [SetUp]
            public void SetUpTest() =>
                _subject = CreateDictionarySubject();

            [Test]
            public void ProviderName_OfProperty() =>
                _subject.ValueOf(x => x.Count)
                    .ProviderName.Should().Be("subject.Count");

            [Test]
            public void ProviderName_OfFunction() =>
                _subject.ValueOf(x => x.ContainsKey("a"))
                    .ProviderName.Should().Be("subject.ContainsKey(\"a\")");

            [Test]
            public void ProviderName_OfProperty_AfterMultipleActions() =>
                _subject
                    .Act(x => x.Add("d", 4))
                    .Act(x => x.Add("e", 5))
                    .ValueOf(x => x.Count).ProviderName.Should().Be("subject{ Add(\"d\", 4); Add(\"e\", 5) }.Count");
        }

        [TestFixture]
        public class ResultOf
        {
            private Subject<Dictionary<string, int>> _subject;

            [SetUp]
            public void SetUpTest() =>
                _subject = CreateDictionarySubject();

            [Test]
            public void Property() =>
                _subject.ResultOf(x => x.Count)
                    .Should.Equal(3);

            [Test]
            public void Function() =>
                _subject.ResultOf(x => x.ContainsKey("a"))
                    .Should.BeTrue();

            [Test]
            public void Function_WithOutParameter()
            {
                int value;

                _subject.ResultOf(x => x.TryGetValue("a", out value))
                    .Should.BeTrue();
            }

            [Test]
            public void Indexer() =>
                _subject.ResultOf(x => x["a"])
                    .Should.Equal(1);

            [Test]
            public void ProviderName_OfProperty() =>
                _subject.ResultOf(x => x.Count)
                    .ProviderName.Should().Be("subject.Count => result");

            [Test]
            public void ProviderName_OfPropertyChain() =>
                _subject.ResultOf(x => x.Keys.Count)
                    .ProviderName.Should().Be("subject.Keys.Count => result");

            [Test]
            public void ProviderName_OfFunction() =>
                _subject.ResultOf(x => x.ContainsKey("a"))
                    .ProviderName.Should().Be("subject.ContainsKey(\"a\") => result");

            [Test]
            public void ProviderName_OfFunction_WithOutParameter()
            {
                int value;

                _subject.ResultOf(x => x.TryGetValue("a", out value))
                    .ProviderName.Should().Be("subject.TryGetValue(\"a\", out value) => result");
            }

            [Test]
            public void ProviderName_OfFunction_AfterAct() =>
                _subject
                    .Act(x => x.Add("d", 4))
                    .ResultOf(x => x.ContainsKey("d"))
                    .ProviderName.Should().Be("subject{ Add(\"d\", 4) }.ContainsKey(\"d\") => result");

            [Test]
            public void ProviderName_OfFunction_AfterMultipleActs() =>
                _subject
                    .Act(x => x.Add("d", 4))
                    .Act(x => x.Add("e", 5))
                    .ResultOf(x => x.ContainsKey("d"))
                    .ProviderName.Should().Be("subject{ Add(\"d\", 4); Add(\"e\", 5) }.ContainsKey(\"d\") => result");

            [Test]
            public void ProviderName_OfFunction_AfterResultOf() =>
                _subject.ResultOf(x => x.Keys.Where(key => key != "z"))
                    .ResultOf(x => x.First())
                    .ProviderName.ToResultSubject()
                    .Should.Equal("subject.Keys.Where(key => key != \"z\") => result.First() => result");

            [Test]
            public void ProviderName_OfIndexer() =>
                _subject.ResultOf(x => x["a"])
                    .ProviderName.Should().Be("subject[\"a\"] => result");
        }

        [TestFixture]
        public class Act
        {
            private Subject<Dictionary<string, int>> _subject;

            [SetUp]
            public void SetUpTest() =>
                _subject = CreateDictionarySubject();

            [Test]
            public void ProviderName_OfAction() =>
                _subject.Act(x => x.Add("d", 4))
                    .ProviderName.Should().Be("subject{ Add(\"d\", 4) }");

            [Test]
            public void ProviderName_OfFunction() =>
                _subject.Act(x => x.Remove("c"))
                    .ProviderName.Should().Be("subject{ Remove(\"c\") }");

            [Test]
            public void ProviderName_OfIndexer() =>
                _subject.Act(x => x["a"] = 1, "[\"a\"] = 1")
                    .ProviderName.Should().Be("subject{ [\"a\"] = 1 }");

            [Test]
            public void ProviderName_OfAction_Multiple() =>
                _subject
                    .Act(x => x.Add("d", 4))
                    .Act(x => x.Add("e", 5))
                    .ProviderName.Should().Be("subject{ Add(\"d\", 4); Add(\"e\", 5) }");

            [Test]
            public void Returns() =>
                _subject.Act(x => x.Clear())
                    .Should.Equal(_subject);
        }
    }
}
