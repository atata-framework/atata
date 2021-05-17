using System.Collections.Generic;
using NUnit.Framework;

namespace Atata.Tests.Utils
{
    [TestFixture]
    public class TemplateStringTransformerTests
    {
        [Test]
        public void Transform_WithStringAndIntVariables()
        {
            string template = "{a}{b}";
            var variables = new Dictionary<string, object>
            {
                ["a"] = "1",
                ["b"] = 2
            };

            Subject.ResultOf(() => TemplateStringTransformer.Transform(template, variables))
                .Should.Equal("12");
        }

        [Test]
        public void Transform_WithIntFormat()
        {
            string template = "-{a:D3}-";
            var variables = new Dictionary<string, object>
            {
                ["a"] = 1
            };

            Subject.ResultOf(() => TemplateStringTransformer.Transform(template, variables))
                .Should.Equal("-001-");
        }

        [Test]
        public void Transform_WithDateTimeFormat()
        {
            string template = "{a:yyyy-MM-dd HH_mm_ss}";
            var variables = new Dictionary<string, object>
            {
                ["a"] = new System.DateTime(2021, 5, 12, 11, 39, 15)
            };

            Subject.ResultOf(() => TemplateStringTransformer.Transform(template, variables))
                .Should.Equal("2021-05-12 11_39_15");
        }

        [Test]
        public void Transform_WithExtendedStringFormat()
        {
            string template = "{a:-*-}";
            var variables = new Dictionary<string, object>
            {
                ["a"] = "1"
            };

            Subject.ResultOf(() => TemplateStringTransformer.Transform(template, variables))
                .Should.Equal("-1-");
        }
    }
}
