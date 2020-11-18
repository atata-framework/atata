using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Atata.Tests.Bahaviors
{
    [TestFixture(typeof(ValueClearUsingClearMethodAttribute))]
    [TestFixture(typeof(ValueClearUsingCtrlADeleteKeysAttribute))]
    [TestFixture(typeof(ValueClearUsingHomeShiftEndDeleteKeysAttribute))]
    [TestFixture(typeof(ValueClearUsingShiftHomeDeleteKeysAttribute))]
    [TestFixture(typeof(ValueClearUsingScriptAttribute))]
    [TestFixture(typeof(ValueClearUsingClearMethodOrScriptAttribute))]
    public class ValueClearBehaviorAttributeTests<TBehaviorAttribute> : UITestFixture
        where TBehaviorAttribute : ValueClearBehaviorAttribute, new()
    {
        [Test]
        public void Execute()
        {
            var sut = Go.To<InputPage>().TextInput;
            sut.Set("abc");

            sut.Metadata.Push(new[] { new TBehaviorAttribute() });

            sut.Clear();

            sut.Should.BeNull();
        }
    }
}
