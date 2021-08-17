using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class SetsValueUsingScriptAttributeTests : UITestFixture
    {
        [Test]
        public void Execute()
        {
            var sut = Go.To<InputPage>().TextInput;
            sut.Set("abc");

            sut.Metadata.Push(new SetsValueUsingScriptAttribute());

            sut.Set("def");

            sut.Should.Equal("def");
        }
    }
}
