namespace Atata.UnitTests;

public class EventBusTests
{
    protected Subject<EventBus> Sut { get; private set; } = null!;

    protected AtataContext Context { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Context = AtataContext.CreateBuilder(AtataContextScope.Test)
            .Build();

        Sut = new EventBus(Context)
            .ToSutSubject();
    }

    public sealed class Publish : EventBusTests
    {
        [Test]
        public void WithNull() =>
            Sut.Invoking(x => x.Publish<TestEvent>(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public void WhenThereIsNoSubscription() =>
            Sut.Invoking(x => x.Publish(new TestEvent()))
                .Should.Not.Throw();

        [Test]
        public void WhenThereIsEventHandler()
        {
            var actionMock = new Mock<Action<TestEvent>>();
            var eventData = new TestEvent();

            Sut.Object.Subscribe(actionMock.Object);

            Sut.Act(x => x.Publish(eventData));

            actionMock.Verify(x => x(eventData), Times.Once);
        }

        [Test]
        public void WhenThereIsAsyncEventHandler()
        {
            var actionMock = new Mock<Func<TestEvent, CancellationToken, Task>>();
            var eventData = new TestEvent();

            Sut.Object.Subscribe(actionMock.Object);

            Sut.Act(x => x.Publish(eventData));

            actionMock.Verify(x => x(eventData, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void WhenThereIsEventHandler_CanHandle_False()
        {
            var conditionalEventHandlerMock = new Mock<IConditionalEventHandler<TestEvent>>(MockBehavior.Strict);
            var eventData = new TestEvent();

            Sut.Object.Subscribe(conditionalEventHandlerMock.Object);

            conditionalEventHandlerMock.Setup(x => x.CanHandle(eventData, Context)).Returns(false);

            Sut.Act(x => x.Publish(eventData));

            conditionalEventHandlerMock.Verify(x => x.CanHandle(eventData, Context), Times.Once);
            conditionalEventHandlerMock.VerifyNoOtherCalls();
        }

        [Test]
        public void WhenThereIsAsyncEventHandler_CanHandle_False()
        {
            var conditionalEventHandlerMock = new Mock<IConditionalAsyncEventHandler<TestEvent>>(MockBehavior.Strict);
            var eventData = new TestEvent();

            Sut.Object.Subscribe(conditionalEventHandlerMock.Object);

            conditionalEventHandlerMock.Setup(x => x.CanHandle(eventData, Context)).Returns(false);

            Sut.Act(x => x.Publish(eventData));

            conditionalEventHandlerMock.Verify(x => x.CanHandle(eventData, Context), Times.Once);
            conditionalEventHandlerMock.VerifyNoOtherCalls();
        }

        [Test]
        public void WhenThereIsEventHandler_CanHandle_True()
        {
            var conditionalEventHandlerMock = new Mock<IConditionalEventHandler<TestEvent>>(MockBehavior.Strict);
            var eventData = new TestEvent();

            Sut.Object.Subscribe(conditionalEventHandlerMock.Object);

            conditionalEventHandlerMock.Setup(x => x.CanHandle(eventData, Context)).Returns(true);
            conditionalEventHandlerMock.Setup(x => x.Handle(eventData, Context));

            Sut.Act(x => x.Publish(eventData));

            conditionalEventHandlerMock.Verify(x => x.CanHandle(eventData, Context), Times.Once);
            conditionalEventHandlerMock.Verify(x => x.Handle(eventData, Context), Times.Once);
            conditionalEventHandlerMock.VerifyNoOtherCalls();
        }

        [Test]
        public void WhenThereIsAsyncEventHandler_CanHandle_True()
        {
            var conditionalEventHandlerMock = new Mock<IConditionalAsyncEventHandler<TestEvent>>(MockBehavior.Strict);
            var eventData = new TestEvent();

            Sut.Object.Subscribe(conditionalEventHandlerMock.Object);

            conditionalEventHandlerMock.Setup(x => x.CanHandle(eventData, Context)).Returns(true);
            conditionalEventHandlerMock.Setup(x => x.HandleAsync(eventData, Context, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            Sut.Act(x => x.Publish(eventData));

            conditionalEventHandlerMock.Verify(x => x.CanHandle(eventData, Context), Times.Once);
            conditionalEventHandlerMock.Verify(x => x.HandleAsync(eventData, Context, It.IsAny<CancellationToken>()), Times.Once);
            conditionalEventHandlerMock.VerifyNoOtherCalls();
        }

        [Test]
        public void WhenThereAreMultipleSubscriptions()
        {
            var actionMock1 = new Mock<Action<TestEvent>>(MockBehavior.Strict);
            var actionMock2 = new Mock<Action<TestEvent, AtataContext>>(MockBehavior.Strict);
            var asyncEventHandlerMock1 = new Mock<IAsyncEventHandler<TestEvent>>(MockBehavior.Strict);
            var syncEventHandlerMock1 = new Mock<IConditionalEventHandler<TestEvent>>(MockBehavior.Strict);
            var syncEventHandlerMock2 = new Mock<IEventHandler<TestEvent>>(MockBehavior.Strict);
            var eventData = new TestEvent();

            Sut.Object.Subscribe(actionMock1.Object);
            Sut.Object.Subscribe(actionMock2.Object);
            Sut.Object.Subscribe(asyncEventHandlerMock1.Object);
            Sut.Object.Subscribe(syncEventHandlerMock1.Object);
            Sut.Object.Subscribe(syncEventHandlerMock2.Object);

            MockSequence sequence = new();
            actionMock1.InSequence(sequence).Setup(x => x(eventData));
            actionMock2.InSequence(sequence).Setup(x => x(eventData, Context));
            asyncEventHandlerMock1.InSequence(sequence).Setup(x => x.HandleAsync(eventData, Context, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            syncEventHandlerMock1.InSequence(sequence).Setup(x => x.CanHandle(eventData, Context)).Returns(true);
            syncEventHandlerMock1.InSequence(sequence).Setup(x => x.Handle(eventData, Context));
            syncEventHandlerMock2.InSequence(sequence).Setup(x => x.Handle(eventData, Context));

            Sut.Act(x => x.Publish(eventData));
        }

        [Test]
        public void AfterUnsubscribe()
        {
            var actionMock1 = new Mock<Action<TestEvent>>();
            var actionMock2 = new Mock<Action<TestEvent, AtataContext>>();
            var eventData = new TestEvent();

            var subscription1 = Sut.Object.Subscribe(actionMock1.Object);
            Sut.Object.Subscribe(actionMock2.Object);
            Sut.Object.Unsubscribe(subscription1);

            Sut.Act(x => x.Publish(eventData));

            actionMock1.Verify(x => x(eventData), Times.Never);
            actionMock2.Verify(x => x(eventData, Context), Times.Once);
        }

        [Test]
        public void AfterUnsubscribeHandler()
        {
            var actionMock = new Mock<Action<TestEvent>>();
            var eventHandlerMock = new Mock<IEventHandler<TestEvent>>();
            var eventData = new TestEvent();

            Sut.Object.Subscribe(actionMock.Object);
            Sut.Object.Subscribe(eventHandlerMock.Object);
            Sut.Object.UnsubscribeHandler(eventHandlerMock.Object);

            Sut.Act(x => x.Publish(eventData));

            actionMock.Verify(x => x(eventData), Times.Once);
            eventHandlerMock.Verify(x => x.Handle(eventData, Context), Times.Never);
        }

        [Test]
        public void AfterUnsubscribeAll()
        {
            var actionMock1 = new Mock<Action<TestEvent>>();
            var actionMock2 = new Mock<Action<TestEvent, AtataContext>>();
            var eventData = new TestEvent();

            Sut.Object.Subscribe(actionMock1.Object);
            Sut.Object.Subscribe(actionMock2.Object);
            Sut.Object.UnsubscribeAll<TestEvent>();

            Sut.Act(x => x.Publish(eventData));

            actionMock1.Verify(x => x(eventData), Times.Never);
            actionMock2.Verify(x => x(eventData, Context), Times.Never);
        }
    }

    public sealed class Subscribe : EventBusTests
    {
        [Test]
        public void WithNullDelegate() =>
            Sut.Invoking(x => x.Subscribe<TestEvent>((null as Action)!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public void WithNullAsyncDelegate() =>
            Sut.Invoking(x => x.Subscribe<TestEvent>((null as Func<CancellationToken, Task>)!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public void WithDelegate()
        {
            var actionMock = new Mock<Action<TestEvent>>();

            Sut.ResultOf(x => x.Subscribe(actionMock.Object))
                .Should.Not.BeNull();
        }

        [Test]
        public void WithAsyncDelegate()
        {
            var actionMock = new Mock<Func<TestEvent, CancellationToken, Task>>();

            Sut.ResultOf(x => x.Subscribe(actionMock.Object))
                .Should.Not.BeNull();
        }
    }

    public sealed class Unsubscribe : EventBusTests
    {
        [Test]
        public void WithNull() =>
            Sut.Invoking(x => x.Unsubscribe(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public void WithSubscriptionObject()
        {
            var actionMock = new Mock<Action<TestEvent>>();

            var subscription = Sut.Object.Subscribe(actionMock.Object);

            Sut.Invoking(x => x.Unsubscribe(subscription))
                .Should.Not.Throw();
        }

        [Test]
        public void WithSubscriptionObject_Twice()
        {
            var actionMock = new Mock<Action<TestEvent>>();

            var subscription = Sut.Object.Subscribe(actionMock.Object);
            Sut.Act(x => x.Unsubscribe(subscription));

            Sut.Invoking(x => x.Unsubscribe(subscription))
                .Should.Not.Throw();
        }
    }

    public sealed class UnsubscribeHandler : EventBusTests
    {
        [Test]
        public void WithNull() =>
            Sut.Invoking(x => x.UnsubscribeHandler(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public void WithSubscriptionHandler()
        {
            var actionMock = new Mock<IEventHandler<TestEvent>>();

            Sut.Object.Subscribe(actionMock.Object);

            Sut.Invoking(x => x.UnsubscribeHandler(actionMock.Object))
                .Should.Not.Throw();
        }

        [Test]
        public void WithSubscriptionHandler_Twice()
        {
            var actionMock = new Mock<IEventHandler<TestEvent>>();

            Sut.Object.Subscribe(actionMock.Object);
            Sut.Act(x => x.UnsubscribeHandler(actionMock.Object));

            Sut.Invoking(x => x.UnsubscribeHandler(actionMock.Object))
                .Should.Not.Throw();
        }
    }

    public class TestEvent
    {
    }
}
