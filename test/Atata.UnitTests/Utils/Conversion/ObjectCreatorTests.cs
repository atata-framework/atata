namespace Atata.UnitTests.Utils.Conversion;

public static class ObjectCreatorTests
{
    public sealed class Create
    {
        private ObjectCreator _sut = null!;

        [SetUp]
        public void SetUp()
        {
            ObjectConverter objectConverter = new();
            ObjectMapper objectMapper = new(objectConverter);

            _sut = new(objectConverter, objectMapper);
        }

        [Test]
        public void Empty()
        {
            object result = _sut.Create(
                typeof(TestClassEmpty),
                []);

            result.Should().BeOfType<TestClassEmpty>();
        }

        [Test]
        public void WithPropertyValues_ForTypeWithDefaultConstructor()
        {
            object result = _sut.Create(
                typeof(TestClassWithProperties),
                new Dictionary<string, object?>
                {
                    ["id"] = 1,
                    ["name"] = "SomeName"
                });

            var castedResult = result.Should().BeOfType<TestClassWithProperties>().Subject;

            using (new AssertionScope())
            {
                castedResult.Id.Should().Be(1);
                castedResult.Name.Should().Be("SomeName");
            }
        }

        [Test]
        public void WithPropertyValues_ForTypeWithoutDefaultConstructor()
        {
            object result = _sut.Create(
                typeof(TestClassWithConstructorsAndProperties),
                new Dictionary<string, object?>
                {
                    ["id"] = 1,
                    ["name"] = "SomeName"
                });

            var castedResult = result.Should().BeOfType<TestClassWithConstructorsAndProperties>().Subject;

            using (new AssertionScope())
            {
                castedResult.Id.Should().Be(1);
                castedResult.Name.Should().Be("SomeName");
            }
        }

        [Test]
        public void WithConstructorParametersAndPropertyValues()
        {
            object result = _sut.Create(
                typeof(TestClassWithConstructorsAndProperties),
                new Dictionary<string, object?>
                {
                    ["id"] = 1,
                    ["keys"] = new[] { "a", "b" },
                    ["name"] = "SomeName"
                });

            var castedResult = result.Should().BeOfType<TestClassWithConstructorsAndProperties>().Subject;

            using (new AssertionScope())
            {
                castedResult.Id.Should().Be(1);
                castedResult.Keys.Should().Equal("a", "b");
                castedResult.Name.Should().Be("SomeName");
            }
        }

        [Test]
        public void WithAlternativeConstructorParameterName()
        {
            object result = _sut.Create(
                typeof(TestClassWithConstructorsAndProperties),
                new Dictionary<string, object?>
                {
                    ["id"] = 1,
                    ["keysCustom"] = new[] { "a", "b" }
                },
                new Dictionary<string, string>
                {
                    ["keysCustom"] = "keys",
                    ["nameCustom"] = "name"
                });

            var castedResult = result.Should().BeOfType<TestClassWithConstructorsAndProperties>().Subject;

            using (new AssertionScope())
            {
                castedResult.Id.Should().Be(1);
                castedResult.Keys.Should().Equal("a", "b");
            }
        }

        [SuppressMessage("Minor Code Smell", "S2094:Classes should not be empty")]
        public sealed class TestClassEmpty;

        public sealed class TestClassWithProperties
        {
            public int Id { get; set; }

            public required string Name { get; init; }
        }

        public sealed class TestClassWithConstructorsAndProperties
        {
            public TestClassWithConstructorsAndProperties(params string[] keys) =>
                Keys = keys;

            public TestClassWithConstructorsAndProperties(int id, params string[] keys) =>
               (Id, Keys) = (id, keys);

            public string[] Keys { get; }

            public int Id { get; set; }

            public required string Name { get; init; }
        }
    }
}
