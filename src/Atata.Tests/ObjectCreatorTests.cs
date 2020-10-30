using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class ObjectCreatorTests
    {
        private ObjectCreator sut;

        [SetUp]
        public void SetUp()
        {
            IObjectConverter objectConverter = new ObjectConverter();
            IObjectMapper objectMapper = new ObjectMapper(objectConverter);

            sut = new ObjectCreator(objectConverter, objectMapper);
        }

        [Test]
        public void ObjectCreator_Create_Empty()
        {
            object result = sut.Create(
                typeof(IgnoreInitAttribute),
                new Dictionary<string, object>());

            result.Should().BeOfType<IgnoreInitAttribute>();
        }

        [Test]
        public void ObjectCreator_Create_WithPropertyValues_ForTypeWithDefaultConstructor()
        {
            object result = sut.Create(
                typeof(TraceLogAttribute),
                new Dictionary<string, object>
                {
                    ["targetName"] = "SomeName",
                    ["targetParentTypes"] = new[] { nameof(InputPage), nameof(TablePage) }
                });

            var castedResult = result.Should().BeOfType<TraceLogAttribute>().Subject;

            using (new AssertionScope())
            {
                castedResult.TargetNames.Should().Equal("SomeName");
                castedResult.TargetParentTypes.Should().Equal(typeof(InputPage), typeof(TablePage));
            }
        }

        [Test]
        public void ObjectCreator_Create_WithPropertyValues_ForTypeWithoutDefaultConstructor()
        {
            object result = sut.Create(
                typeof(FindByIdAttribute),
                new Dictionary<string, object>
                {
                    ["targetName"] = "SomeName",
                    ["targetParentTypes"] = new[] { nameof(InputPage), nameof(TablePage) }
                });

            var castedResult = result.Should().BeOfType<FindByIdAttribute>().Subject;

            using (new AssertionScope())
            {
                castedResult.TargetNames.Should().Equal("SomeName");
                castedResult.TargetParentTypes.Should().Equal(typeof(InputPage), typeof(TablePage));
            }
        }

        [Test]
        public void ObjectCreator_Create_WithConstructorParametersAndPropertyValues()
        {
            object result = sut.Create(
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
                castedResult.Values.Should().Equal(new[] { "val1", "val2" });
                castedResult.Format.Should().Be("{0}!");
            }
        }

        [Test]
        public void ObjectCreator_Create_WithAlternativeConstructorParameterName_Value()
        {
            object result = sut.Create(
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
                castedResult.Values.Should().Equal(new[] { "val1" });
            }
        }

        [Test]
        public void ObjectCreator_Create_WithAlternativeConstructorParameterName_Case()
        {
            object result = sut.Create(
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
