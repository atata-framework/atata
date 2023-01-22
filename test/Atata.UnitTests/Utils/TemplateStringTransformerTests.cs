namespace Atata.UnitTests.Utils;

public static class TemplateStringTransformerTests
{
    [TestFixture]
    public class Transform
    {
        [Test]
        public void WithMissingVariables()
        {
            string template = "{a}{b}{b}{c}{c:D2}";
            var variables = new Dictionary<string, object>
            {
                ["a"] = 1
            };

            Act(template, variables)
                .Should.Throw<FormatException>()
                .ValueOf(x => x.Message).Should.Be($"Template \"{template}\" string contains unknown variable(s): {{b}}, {{c}}, {{c:D2}}.");
        }

        [Test]
        public void WithIncorrectFormat()
        {
            string template = "{a";
            var variables = new Dictionary<string, object>
            {
                ["a"] = 1
            };

            Act(template, variables)
                .Should.Throw<FormatException>()
                .ValueOf(x => x.Message).Should.Be($"Template \"{template}\" string is not in a correct format.");
        }

        [TestCase("a{{b", ExpectedResult = "a{b")]
        [TestCase("a}}b", ExpectedResult = "a}b")]
        [TestCase("a{{b}}c", ExpectedResult = "a{b}c")]
        [TestCase("{{{a}}}", ExpectedResult = "{1}")]
        public string WithDoubleBraces(string template)
        {
            var variables = new Dictionary<string, object>
            {
                ["a"] = 1
            };

            return Act(template, variables);
        }

        [Test]
        public void WithStringAndIntVariables()
        {
            string template = "{a}{b}";
            var variables = new Dictionary<string, object>
            {
                ["a"] = "1",
                ["b"] = 2
            };

            Act(template, variables)
                .Should.Equal("12");
        }

        [Test]
        public void WithIntFormat()
        {
            string template = "-{a:D3}-";
            var variables = new Dictionary<string, object>
            {
                ["a"] = 1
            };

            Act(template, variables)
                .Should.Equal("-001-");
        }

        [Test]
        public void WithDateTimeFormat()
        {
            string template = "{a:yyyy-MM-dd HH_mm_ss}";
            var variables = new Dictionary<string, object>
            {
                ["a"] = new DateTime(2021, 5, 12, 11, 39, 15)
            };

            Act(template, variables)
                .Should.Equal("2021-05-12 11_39_15");
        }

        [Test]
        public void WithExtendedStringFormat()
        {
            string template = "{a:-*-}";
            var variables = new Dictionary<string, object>
            {
                ["a"] = "1"
            };

            Act(template, variables)
                .Should.Equal("-1-");
        }

        private static Subject<string> Act(string template, Dictionary<string, object> variables) =>
            Subject.ResultOf(() => TemplateStringTransformer.Transform(template, variables));
    }

    [TestFixture]
    public class TransformPath
    {
        [Test]
        public void WithStringAndIntVariables()
        {
            string template = "{a}{b}";
            var variables = new Dictionary<string, object>
            {
                ["a"] = "1",
                ["b"] = 2
            };

            Act(template, variables)
                .Should.Equal("12");
        }

        [Test]
        public void WithVariablesContainingInvalidChars()
        {
            string template = "{a}-{b}";
            var variables = new Dictionary<string, object>
            {
                ["a"] = "10:30",
                ["b"] = "1*2/3=?"
            };

            Act(template, variables)
                .Should.Equal("10_30-1_2_3=_");
        }

        private static Subject<string> Act(string template, Dictionary<string, object> variables) =>
            Subject.ResultOf(() => TemplateStringTransformer.TransformPath(template, variables));
    }
}
