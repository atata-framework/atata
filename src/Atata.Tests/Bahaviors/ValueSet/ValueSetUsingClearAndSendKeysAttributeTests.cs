using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class ValueSetUsingClearAndSendKeysAttributeTests : UITestFixture
    {
        [Test]
        public void ValueSetUsingClearAndSendKeysAttribute_Execute()
        {
            var sut = Go.To<InputPage>().TextInput;
            sut.Set("abc");

            sut.Metadata.Push(new ValueSetUsingClearAndSendKeysAttribute());

            sut.Set("def");

            sut.Should.Equal("def");
        }
    }
}
