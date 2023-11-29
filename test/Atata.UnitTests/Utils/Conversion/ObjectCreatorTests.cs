namespace Atata.UnitTests.Utils.Conversion;

public static class ObjectCreatorTests
{
    [TestFixture]
    public class Create
    {
        private ObjectCreator _sut;

        [SetUp]
        public void SetUp()
        {
            IObjectConverter objectConverter = new ObjectConverter();
            IObjectMapper objectMapper = new ObjectMapper(objectConverter);

            _sut = new ObjectCreator(objectConverter, objectMapper);
        }

        [Test]
        public void Empty()
        {
            object result = _sut.Create(
                typeof(IgnoreInitAttribute),
                []);

            result.Should().BeOfType<IgnoreInitAttribute>();
        }

        [Test]
        public void WithPropertyValues_ForTypeWithDefaultConstructor()
        {
            object result = _sut.Create(
                typeof(TraceLogAttribute),
                new Dictionary<string, object>
                {
                    ["targetName"] = "SomeName",
                    ["targetParentTypes"] = new[] { nameof(OrdinaryPage), nameof(TestPage) }
                });

            var castedResult = result.Should().BeOfType<TraceLogAttribute>().Subject;

            using (new AssertionScope())
            {
                castedResult.TargetNames.Should().Equal("SomeName");
                castedResult.TargetParentTypes.Should().Equal(typeof(OrdinaryPage), typeof(TestPage));
            }
        }

        [Test]
        public void WithPropertyValues_ForTypeWithoutDefaultConstructor()
        {
            object result = _sut.Create(
                typeof(FindByIdAttribute),
                new Dictionary<string, object>
                {
                    ["targetName"] = "SomeName",
                    ["targetParentTypes"] = new[] { nameof(OrdinaryPage), nameof(TestPage) }
                });

            var castedResult = result.Should().BeOfType<FindByIdAttribute>().Subject;

            using (new AssertionScope())
            {
                castedResult.TargetNames.Should().Equal("SomeName");
                castedResult.TargetParentTypes.Should().Equal(typeof(OrdinaryPage), typeof(TestPage));
            }
        }

        [Test]
        public void WithConstructorParametersAndPropertyValues()
        {
            object result = _sut.Create(
                typeof(FindByIdAttribute),
                new Dictionary<string, object>
                {
                    ["match"] = TermMatch.StartsWith,
                    ["values"] = new[] { "val1", "val2" },
                    ["format"] = "{0}!"
                });

            var castedResult = result.Should().BeOfType<FindByIdAttribute>().Subject;

            using (new AssertionScope())
            {
                castedResult.Match.Should().Be(TermMatch.StartsWith);
                castedResult.Values.Should().Equal("val1", "val2");
                castedResult.Format.Should().Be("{0}!");
            }
        }

        [Test]
        public void WithAlternativeConstructorParameterName_Value()
        {
            object result = _sut.Create(
                typeof(FindByIdAttribute),
                new Dictionary<string, object>
                {
                    ["match"] = TermMatch.EndsWith,
                    ["value"] = "val1"
                },
                new Dictionary<string, string>
                {
                    ["value"] = "values",
                    ["case"] = "termCase"
                });

            var castedResult = result.Should().BeOfType<FindByIdAttribute>().Subject;

            using (new AssertionScope())
            {
                castedResult.Match.Should().Be(TermMatch.EndsWith);
                castedResult.Values.Should().Equal("val1");
            }
        }

        [Test]
        public void WithAlternativeConstructorParameterName_Case()
        {
            object result = _sut.Create(
                typeof(FindByIdAttribute),
                new Dictionary<string, object>
                {
                    ["case"] = TermCase.LowerMerged,
                    ["match"] = TermMatch.EndsWith
                },
                new Dictionary<string, string>
                {
                    ["value"] = "values",
                    ["case"] = "termCase"
                });

            var castedResult = result.Should().BeOfType<FindByIdAttribute>().Subject;

            using (new AssertionScope())
            {
                castedResult.Match.Should().Be(TermMatch.EndsWith);
                castedResult.Case.Should().Be(TermCase.LowerMerged);
            }
        }
    }
}
