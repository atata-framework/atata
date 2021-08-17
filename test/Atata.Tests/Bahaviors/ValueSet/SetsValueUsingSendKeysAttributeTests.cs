using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class SetsValueUsingSendKeysAttributeTests : UITestFixture
    {
        [Test]
        public void Execute()
        {
            var sut = Go.To<InputPage>().TextInput;
            sut.Set("abc");

            sut.Metadata.Push(new SetsValueUsingSendKeysAttribute());

            sut.Set("def");

            sut.Should.Equal("abcdef");
        }
    }
}
