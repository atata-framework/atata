using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class ValueSetUsingSendKeysAttributeTests : UITestFixture
    {
        [Test]
        public void ValueSetUsingSendKeysAttribute_Execute()
        {
            var sut = Go.To<InputPage>().TextInput;
            sut.Set("abc");

            sut.Metadata.Push(new ValueSetUsingSendKeysAttribute());

            sut.Set("def");

            sut.Should.Equal("abcdef");
        }
    }
}
