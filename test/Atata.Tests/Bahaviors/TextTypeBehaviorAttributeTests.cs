using System.Collections.Generic;
using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class TextTypeBehaviorAttributeTests : UITestFixture
    {
        private const string InitialValue = "abc";

        private const string SetValue = "def";

        private const string ConcatValue = InitialValue + SetValue;

        private static IEnumerable<TestCaseData> Source =>
            new[]
            {
                new TestCaseData(new TypesTextUsingSendKeysAttribute()).Returns(ConcatValue),
                new TestCaseData(new TypesTextUsingScriptAttribute()).Returns(ConcatValue)
            };

        [TestCaseSource(nameof(Source))]
        public string Execute(TextTypeBehaviorAttribute behavior)
        {
            var sut = Go.To<InputPage>().TextInput;
            sut.Set(InitialValue);

            sut.Metadata.Push(behavior);

            sut.Type(SetValue);

            return sut.Value;
        }
    }
}
