using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Atata.Tests.Bahaviors
{
    [TestFixture(typeof(ClearsValueUsingClearMethodAttribute))]
    [TestFixture(typeof(ClearsValueUsingCtrlADeleteKeysAttribute))]
    [TestFixture(typeof(ClearsValueUsingHomeShiftEndDeleteKeysAttribute))]
    [TestFixture(typeof(ClearsValueUsingShiftHomeDeleteKeysAttribute))]
    [TestFixture(typeof(ClearsValueUsingScriptAttribute))]
    [TestFixture(typeof(ClearsValueUsingClearMethodOrScriptAttribute))]
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
