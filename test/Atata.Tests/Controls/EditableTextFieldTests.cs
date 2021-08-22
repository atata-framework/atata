using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Atata.Tests.Controls
{
    public class EditableTextFieldTests : UITestFixture
    {
        private EditableTextField<string, InputPage> _sut;

        protected override void OnSetUp()
        {
            base.OnSetUp();

            _sut = Go.To<InputPage>()
                .Controls.Create<EditableTextField<string, InputPage>>("sut", new FindByXPathAttribute("input[@type='text']"));
        }

        [Test]
        public void Get_ExecutesBehavior()
        {
            var behaviorMock = new Mock<ValueGetBehaviorAttribute> { CallBase = true };
            behaviorMock.Setup(x => x.Execute(_sut))
                .Returns("abc")
                .Verifiable();
            _sut.Metadata.Push(behaviorMock.Object);

            _sut.Get().Should().Be("abc");

            behaviorMock.Verify();
        }

        [Test]
        public void Set_ExecutesBehavior()
        {
            var behaviorMock = new Mock<ValueSetBehaviorAttribute> { CallBase = true };
            _sut.Metadata.Push(behaviorMock.Object);

            _sut.Set("abc");

            behaviorMock.Verify(x => x.Execute(_sut, "abc"), Times.Once);
        }

        [Test]
        public void Type_ExecutesBehavior()
        {
            var behaviorMock = new Mock<TextTypeBehaviorAttribute> { CallBase = true };
            _sut.Metadata.Push(behaviorMock.Object);

            _sut.Type("abc");

            behaviorMock.Verify(x => x.Execute(_sut, "abc"), Times.Once);
        }

        [Test]
        public void Clear_ExecutesBehavior()
        {
            var behaviorMock = new Mock<ValueClearBehaviorAttribute> { CallBase = true };
            _sut.Metadata.Push(behaviorMock.Object);

            _sut.Clear();

            behaviorMock.Verify(x => x.Execute(_sut), Times.Once);
        }
    }
}
