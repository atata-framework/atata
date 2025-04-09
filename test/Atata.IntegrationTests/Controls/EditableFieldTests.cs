namespace Atata.IntegrationTests.Controls;

public static class EditableFieldTests
{
    public class GetSet : WebDriverSessionTestSuite
    {
        [Test]
        public void WithValueGetFormatAttribute()
        {
            var sut = Go.To<InputPage>().TextInput;

            sut.Metadata.Push(
                new ValueGetFormatAttribute("INDICATOR {0}"));

            sut.Set("INDICATOR abc");

            sut.Should.AtOnce.Be("abc");
            sut.DomProperties.Value.Should.AtOnce.Be("INDICATOR abc");
        }

        [Test]
        public void WithValueSetFormatAttribute()
        {
            var sut = Go.To<InputPage>().TextInput;

            sut.Metadata.Push(
                new ValueSetFormatAttribute("INDICATOR {0}"));

            sut.Set("abc");

            sut.Should.AtOnce.Be("INDICATOR abc");
            sut.DomProperties.Value.Should.AtOnce.Be("INDICATOR abc");
        }

        [Test]
        public void WithValueGetFormatAttribute_AndValueSetFormatAttribute()
        {
            var sut = Go.To<InputPage>().TextInput;

            sut.Metadata.Push(
                new ValueSetFormatAttribute("SET {0}"),
                new ValueGetFormatAttribute("SET INDICATOR {0}"));

            sut.Set("INDICATOR abc");

            sut.Should.AtOnce.Be("abc");
            sut.DomProperties.Value.Should.AtOnce.Be("SET INDICATOR abc");
        }

        [Test]
        public void WithFormatAttribute_AndValueGetFormatAttribute_AndValueSetFormatAttribute()
        {
            var sut = Go.To<InputPage>().TextInputWithFormat;

            sut.Metadata.Push(
                new ValueSetFormatAttribute("SET {0}"),
                new ValueGetFormatAttribute("SET INDICATOR {0}"));

            sut.Set("INDICATOR abc");

            sut.Should.AtOnce.Be("abc");
            sut.DomProperties.Value.Should.AtOnce.Be("SET INDICATOR abc");
        }

        [Test]
        public void WithFormatAttribute_AndValueSetFormatAttribute()
        {
            var sut = Go.To<InputPage>().TextInputWithFormat;

            sut.Metadata.Push(
                new ValueSetFormatAttribute("!SET {0}!"));

            sut.Set("abc");

            sut.Should.AtOnce.Be("SET abc");
            sut.DomProperties.Value.Should.AtOnce.Be("!SET abc!");
        }
    }
}
