namespace Atata.UnitTests.Utils;

public static class TemplateStringTransformerTests
{
    public class Transform
    {
        [Test]
        public void WithMissingVariables()
        {
            string template = "{a}{b}{b}{c}{c:D2}";
            var variables = new Dictionary<string, object?>
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
            var variables = new Dictionary<string, object?>
            {
                ["a"] = 1
            };

            Act(template, variables)
                .Should.Throw<FormatException>()
                .ValueOf(x => x.Message).Should.Be($"Template \"{template}\" string is not in a correct format.");
        }

        [Test]
        public void WithNullVariable()
        {
            string template = "-{a}-";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = null
            };

            Act(template, variables)
                .Should.Be("--");
        }

        [Test]
        public void WithNullVariable_AndExtendedStringFormat()
        {
            string template = "-{a:+*+}-";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = null
            };

            Act(template, variables)
                .Should.Be("--");
        }

        [TestCase("a{{b", ExpectedResult = "a{b")]
        [TestCase("a}}b", ExpectedResult = "a}b")]
        [TestCase("a{{b}}c", ExpectedResult = "a{b}c")]
        [TestCase("{{{a}}}", ExpectedResult = "{1}")]
        public string WithDoubleBraces(string template)
        {
            var variables = new Dictionary<string, object?>
            {
                ["a"] = 1
            };

            return Act(template, variables);
        }

        [Test]
        public void WithStringAndIntVariables()
        {
            string template = "{a}{b}";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = "1",
                ["b"] = 2
            };

            Act(template, variables)
                .Should.Be("12");
        }

        [Test]
        public void WithIntFormat()
        {
            string template = "-{a:D3}-";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = 1
            };

            Act(template, variables)
                .Should.Be("-001-");
        }

        [Test]
        public void WithDateTimeFormat()
        {
            string template = "{a:yyyy-MM-dd HH_mm_ss}";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = new DateTime(2021, 5, 12, 11, 39, 15)
            };

            Act(template, variables)
                .Should.Be("2021-05-12 11_39_15");
        }

        [Test]
        public void WithExtendedStringFormat()
        {
            string template = "{a:-*-}";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = "1"
            };

            Act(template, variables)
                .Should.Be("-1-");
        }

        [Test]
        public void WithExtendedStringFormatThatHasDoubleStars()
        {
            string template = "{a:** * **}";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = "1"
            };

            Act(template, variables)
                .Should.Be("* 1 *");
        }

        private static Subject<string> Act(string template, Dictionary<string, object?> variables) =>
            Subject.ResultOf(() => TemplateStringTransformer.Transform(template, variables));
    }

    public sealed class TransformPath
    {
        [Test]
        public void WithStringAndIntVariables()
        {
            string template = "{a}{b}";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = "1",
                ["b"] = 2
            };

            Act(template, variables)
                .Should.Be("12");
        }

        [Test]
        public void WithVariablesContainingInvalidChars()
        {
            string template = "{a}-{b}";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = "10:30",
                ["b"] = "1*2/3=?"
            };

            Act(template, variables)
                .Should.Be("10_30-1_2_3=_");
        }

        [Test]
        public void WithExtendedStringFormat()
        {
            string template = "{a:-*/\\}";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = "a/\\b"
            };

            Act(template, variables)
                .Should.Be("-a__b/\\");
        }

        [Test]
        public void WithExtendedStringFormatThatHasDoubleStars()
        {
            string template = "{a:** * **}";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = "1"
            };

            Act(template, variables)
                .Should.Be("* 1 *");
        }

        private static Subject<string> Act(string template, Dictionary<string, object?> variables) =>
            Subject.ResultOf(() => TemplateStringTransformer.TransformPath(template, variables));
    }

    public sealed class TransformUri
    {
        [Test]
        public void WithNormalVariables()
        {
            string template = "{a}{b}";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = "1",
                ["b"] = 2
            };

            Act(template, variables)
                .Should.Be("12");
        }

        [Test]
        public void WithVariablesToBeDataEscaped()
        {
            string template = "{a}-{b}?q=z{c:dataescape:#*}{c:#*}{c:#*:dataescape}";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = "10:30",
                ["b"] = "1*2/3=?",
                ["c"] = "frg?"
            };

            Act(template, variables)
                .Should.Be("10%3A30-1%2A2%2F3%3D%3F?q=z#frg%3F%23frg%3F%23frg%3F");
        }

        [Test]
        public void WithVariablesToBeNotEscaped()
        {
            string template = "/path/{a:d3:noescape}/{b:noescape}{c:_*_:noescape}{c:noescape:_*_}#frg";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = 42,
                ["b"] = "x/?q=1&r",
                ["c"] = null
            };

            Act(template, variables)
                .Should.Be("/path/042/x/?q=1&r#frg");
        }

        [Test]
        public void WithVariablesToBeUriEscaped()
        {
            string template = "/path{a:/*:uriescape}/{b:uriescape}#frg";
            var variables = new Dictionary<string, object?>
            {
                ["a"] = "</>",
                ["b"] = "x/?q=1%2&r"
            };

            Act(template, variables)
                .Should.Be("/path/%3C/%3E/x/?q=1%252&r#frg");
        }

        private static Subject<string> Act(string template, Dictionary<string, object?> variables) =>
            Subject.ResultOf(() => TemplateStringTransformer.TransformUri(template, variables));
    }
}
