using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class ValueSetUsingScriptAttributeTests : UITestFixture
    {
        [Test]
        public void ValueSetUsingScriptAttribute_Execute()
        {
            var sut = Go.To<InputPage>().TextInput;
            sut.Set("abc");

            sut.Metadata.Push(new ValueSetUsingScriptAttribute());

            sut.Set("def");

            sut.Should.Equal("def");
        }
    }
}
