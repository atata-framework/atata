using System;
using NUnit.Framework;

namespace Atata.Tests
{
    public static class EventSubscriptionsAtataContextBuilderTests
    {
        [TestFixture]
        public class Add
        {
            private Subject<EventSubscriptionsAtataContextBuilder> _sut;

            [SetUp]
            public void SetUp() =>
                _sut = new EventSubscriptionsAtataContextBuilder(new AtataBuildingContext())
                    .ToSutSubject();

            [Test]
            public void NullAsAction() =>
                _sut.Invoking(x => x.Add<TestEvent>(null as Action))
                    .Should.Throw<ArgumentNullException>();

            [Test]
            public void Action() =>
                _sut.Act(x => x.Add<TestEvent>(StubMethod))
                    .ResultOf(x => x.BuildingContext.EventSubscriptions)
                        .Should.ContainSingle()
                        .Single().Should.Satisfy(
                            x => x.EventType == typeof(TestEvent) && x.EventHandler != null);

            [Test]
            public void ActionWith1GenericParemeter() =>
                _sut.Act(x => x.Add<TestEvent>(x => StubMethod()))
                    .ResultOf(x => x.BuildingContext.EventSubscriptions)
                        .Should.ContainSingle()
                        .Single().Should.Satisfy(
                            x => x.EventType == typeof(TestEvent) && x.EventHandler != null);

            [Test]
            public void ActionWith2GenericParemeters() =>
                _sut.Act(x => x.Add<TestEvent>((x, c) => StubMethod()))
                    .ResultOf(x => x.BuildingContext.EventSubscriptions)
                        .Should.ContainSingle()
                        .Single().Should.Satisfy(
                            x => x.EventType == typeof(TestEvent) && x.EventHandler != null);

            [Test]
            public void TwoGenericParameters() =>
                _sut.Act(x => x.Add<TestEvent, TestEventHandler>())
                    .ResultOf(x => x.BuildingContext.EventSubscriptions)
                        .Should.ContainSingle()
                        .Single().Should.Satisfy(
                            x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

            [Test]
            public void EventHandler()
            {
                var eventHandler = new TestEventHandler();

                _sut.Act(x => x.Add(eventHandler))
                    .ResultOf(x => x.BuildingContext.EventSubscriptions)
                        .Should.ContainSingle()
                        .Single().Should.Satisfy(
                            x => x.EventType == typeof(TestEvent) && x.EventHandler == eventHandler);
            }

            [Test]
            public void EventHandler_Multiple()
            {
                var eventHandler1 = new TestEventHandler();
                var eventHandler2 = new UniversalEventHandler();

                _sut.Act(x => x.Add(eventHandler1))
                    .Act(x => x.Add<TestEvent>(eventHandler2))
                    .ResultOf(x => x.BuildingContext.EventSubscriptions)
                        .Should.HaveCount(2)
                        .ElementAt(0).Should.Satisfy(
                            x => x.EventType == typeof(TestEvent) && x.EventHandler == eventHandler1)
                        .ElementAt(1).Should.Satisfy(
                            x => x.EventType == typeof(TestEvent) && x.EventHandler == eventHandler2);
            }

            [Test]
            public void EventHandlerType() =>
                _sut.Act(x => x.Add(typeof(TestEventHandler)))
                    .ResultOf(x => x.BuildingContext.EventSubscriptions)
                        .Should.ContainSingle()
                        .Single().Should.Satisfy(
                            x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

            [Test]
            public void EventHandlerType_WithInvalidValue() =>
                _sut.Invoking(x => x.Add(typeof(EventSubscriptionsAtataContextBuilderTests)))
                    .Should.Throw<ArgumentException>();

            [Test]
            public void EventTypeAndEventHandlerType_WithExactEventHandlerType() =>
                _sut.Act(x => x.Add(typeof(TestEvent), typeof(TestEventHandler)))
                    .ResultOf(x => x.BuildingContext.EventSubscriptions)
                        .Should.ContainSingle()
                        .Single().Should.Satisfy(
                            x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

            [Test]
            public void EventTypeAndEventHandlerType_WithBaseEventHandlerType() =>
                _sut.Act(x => x.Add(typeof(TestEvent), typeof(UniversalEventHandler)))
                    .ResultOf(x => x.BuildingContext.EventSubscriptions)
                        .Should.ContainSingle()
                        .Single().Should.Satisfy(
                            x => x.EventType == typeof(TestEvent) && x.EventHandler is UniversalEventHandler);

            [Test]
            public void EventTypeAndEventHandlerType_WithInvalidEventHandlerType() =>
                _sut.Invoking(x => x.Add(typeof(TestEvent), typeof(EventSubscriptionsAtataContextBuilderTests)))
                    .Should.Throw<ArgumentException>();

            private static void StubMethod()
            {
                // Method intentionally left empty.
            }

            public class TestEvent
            {
            }

            private class TestEventHandler : IEventHandler<TestEvent>
            {
                public void Handle(TestEvent eventData, AtataContext context)
                {
                    // Method intentionally left empty.
                }
            }

            private class UniversalEventHandler : IEventHandler<object>
            {
                public void Handle(object eventData, AtataContext context)
                {
                    // Method intentionally left empty.
                }
            }
        }
    }
}
