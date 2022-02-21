using System;
using Moq;
using NUnit.Framework;

namespace Atata.Tests
{
    public class EventBusTests
    {
        protected Subject<EventBus> Sut { get; private set; }

        protected AtataContext Context { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Context = AtataContext.Configure()
                .UseDriverInitializationStage(AtataContextDriverInitializationStage.None)
                .Build();

            Sut = new EventBus(Context)
                .ToSutSubject();
        }

        [TestFixture]
        public class Publish : EventBusTests
        {
            [Test]
            public void Null() =>
                Sut.Invoking(x => x.Publish<TestEvent>(null))
                    .Should.Throw<ArgumentNullException>();

            [Test]
            public void WhenThereIsNoSubscription() =>
                Sut.Invoking(x => x.Publish(new TestEvent()))
                    .Should.Not.Throw();

            [Test]
            public void WhenThereIsSubscription()
            {
                var actionMock = new Mock<Action<TestEvent>>();
                var eventData = new TestEvent();

                Sut.Object.Subscribe(actionMock.Object);

                Sut.Act(x => x.Publish(eventData));

                actionMock.Verify(x => x(eventData), Times.Once);
            }

            [Test]
            public void WhenThereAreMultipleSubscriptions()
            {
                var actionMock1 = new Mock<Action<TestEvent>>();
                var actionMock2 = new Mock<Action<TestEvent, AtataContext>>();
                var eventHandlerMock = new Mock<IEventHandler<TestEvent>>();
                var eventData = new TestEvent();

                Sut.Object.Subscribe(actionMock1.Object);
                Sut.Object.Subscribe(actionMock2.Object);
                Sut.Object.Subscribe(eventHandlerMock.Object);

                Sut.Act(x => x.Publish(eventData));

                actionMock1.Verify(x => x(eventData), Times.Once);
                actionMock2.Verify(x => x(eventData, Context), Times.Once);
                eventHandlerMock.Verify(x => x.Handle(eventData, Context), Times.Once);
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

        [TestFixture]
        public class Subscribe : EventBusTests
        {
            [Test]
            public void Action_Null() =>
                Sut.Invoking(x => x.Subscribe<TestEvent>(null as Action))
                    .Should.Throw<ArgumentNullException>();

            [Test]
            public void Action()
            {
                var actionMock = new Mock<Action<TestEvent>>();

                Sut.ResultOf(x => x.Subscribe(actionMock.Object))
                    .Should.Not.BeNull();
            }
        }

        [TestFixture]
        public class Unsubscribe : EventBusTests
        {
            [Test]
            public void Null() =>
                Sut.Invoking(x => x.Unsubscribe(null))
                    .Should.Throw<ArgumentNullException>();

            [Test]
            public void Valid()
            {
                var actionMock = new Mock<Action<TestEvent>>();

                var subscription = Sut.Object.Subscribe(actionMock.Object);

                Sut.Invoking(x => x.Unsubscribe(subscription))
                    .Should.Not.Throw();
            }

            [Test]
            public void Twice()
            {
                var actionMock = new Mock<Action<TestEvent>>();

                var subscription = Sut.Object.Subscribe(actionMock.Object);
                Sut.Act(x => x.Unsubscribe(subscription));

                Sut.Invoking(x => x.Unsubscribe(subscription))
                    .Should.Not.Throw();
            }
        }

        [TestFixture]
        public class UnsubscribeHandler : EventBusTests
        {
            [Test]
            public void Null() =>
                Sut.Invoking(x => x.UnsubscribeHandler(null))
                    .Should.Throw<ArgumentNullException>();

            [Test]
            public void Valid()
            {
                var actionMock = new Mock<IEventHandler<TestEvent>>();

                Sut.Object.Subscribe(actionMock.Object);

                Sut.Invoking(x => x.UnsubscribeHandler(actionMock.Object))
                    .Should.Not.Throw();
            }

            [Test]
            public void Twice()
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
}
