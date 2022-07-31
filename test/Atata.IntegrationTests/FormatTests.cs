using NUnit.Framework;

namespace Atata.IntegrationTests
{
    public class FormatTests : UITestFixture
    {
        [Test]
        public void Format_WithValueGetFormat()
        {
            var control = Go.To<InputPage>().TextInput;

            control.Metadata.Push(
                new ValueGetFormatAttribute("INDICATOR {0}"));

            control.Set("INDICATOR abc");

            control.Should.AtOnce.Equal("abc");
            control.Attributes.Value.Should.AtOnce.Equal("INDICATOR abc");
        }

        [Test]
        public void Format_WithValueSetFormat()
        {
            var control = Go.To<InputPage>().TextInput;

            control.Metadata.Push(
                new ValueSetFormatAttribute("INDICATOR {0}"));

            control.Set("abc");

            control.Should.AtOnce.Equal("INDICATOR abc");
            control.Attributes.Value.Should.AtOnce.Equal("INDICATOR abc");
        }

        [Test]
        public void Format_WithValueGetFormatAndValueSetFormat()
        {
            var control = Go.To<InputPage>().TextInput;

            control.Metadata.Push(
                new ValueSetFormatAttribute("SET {0}"),
                new ValueGetFormatAttribute("SET INDICATOR {0}"));

            control.Set("INDICATOR abc");

            control.Should.AtOnce.Equal("abc");
            control.Attributes.Value.Should.AtOnce.Equal("SET INDICATOR abc");
        }

        [Test]
        public void Format_WithFormatAndValueGetFormatAndValueSetFormat()
        {
            var control = Go.To<InputPage>().TextInputWithFormat;

            control.Metadata.Push(
                new ValueSetFormatAttribute("SET {0}"),
                new ValueGetFormatAttribute("SET INDICATOR {0}"));

            control.Set("INDICATOR abc");

            control.Should.AtOnce.Equal("abc");
            control.Attributes.Value.Should.AtOnce.Equal("SET INDICATOR abc");
        }

        [Test]
        public void Format_WithFormatAndValueSetFormat()
        {
            var control = Go.To<InputPage>().TextInputWithFormat;

            control.Metadata.Push(
                new ValueSetFormatAttribute("!SET {0}!"));

            control.Set("abc");

            control.Should.AtOnce.Equal("SET abc");
            control.Attributes.Value.Should.AtOnce.Equal("!SET abc!");
        }
    }
}
