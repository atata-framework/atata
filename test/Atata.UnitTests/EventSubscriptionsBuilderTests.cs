namespace Atata.UnitTests;

public static class EventSubscriptionsBuilderTests
{
    [TestFixture]
    public class Add
    {
        private Subject<EventSubscriptionsBuilder> _sut;

        [SetUp]
        public void SetUp() =>
            _sut = new EventSubscriptionsBuilder()
                .ToSutSubject();

        [Test]
        public void NullAsAction() =>
            _sut.Invoking(x => x.Add<TestEvent>(null as Action))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public void Action() =>
            _sut.Act(x => x.Add<TestEvent>(StubMethod))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler != null);

        [Test]
        public void ActionWith1GenericParameter() =>
            _sut.Act(x => x.Add<TestEvent>(x => StubMethod()))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler != null);

        [Test]
        public void ActionWith2GenericParameters() =>
            _sut.Act(x => x.Add<TestEvent>((_, _) => StubMethod()))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler != null);

        [Test]
        public void TwoGenericParameters() =>
            _sut.Act(x => x.Add<TestEvent, TestEventHandler>())
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

        [Test]
        public void EventHandler()
        {
            var eventHandler = new TestEventHandler();

            _sut.Act(x => x.Add(eventHandler))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler == eventHandler);
        }

        [Test]
        public void EventHandler_Multiple()
        {
            var eventHandler1 = new TestEventHandler();
            var eventHandler2 = new UniversalEventHandler();

            _sut.Act(x => x.Add(eventHandler1))
                .Act(x => x.Add<TestEvent>(eventHandler2))
                .ResultOf(x => x.Items)
                    .Should.ConsistSequentiallyOf(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler == eventHandler1,
                        x => x.EventType == typeof(TestEvent) && x.EventHandler == eventHandler2);
        }

        [Test]
        public void EventHandlerType() =>
            _sut.Act(x => x.Add(typeof(TestEventHandler)))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

        [Test]
        public void EventHandlerType_WithInvalidValue() =>
            _sut.Invoking(x => x.Add(typeof(EventSubscriptionsBuilderTests)))
                .Should.Throw<ArgumentException>();

        [Test]
        public void EventTypeAndEventHandlerType_WithExactEventHandlerType() =>
            _sut.Act(x => x.Add<TestEvent, TestEventHandler>())
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

        [Test]
        public void EventTypeAndEventHandlerType_WithBaseEventHandlerType() =>
            _sut.Act(x => x.Add<TestEvent, UniversalEventHandler>())
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is UniversalEventHandler);

        [Test]
        public void EventTypeAndEventHandlerType_WithInvalidEventHandlerType() =>
            _sut.Invoking(x => x.Add(typeof(TestEvent), typeof(EventSubscriptionsBuilderTests)))
                .Should.Throw<ArgumentException>();

        private static void StubMethod()
        {
            // Method intentionally left empty.
        }

        public sealed class TestEvent
        {
        }

        private sealed class TestEventHandler : IEventHandler<TestEvent>
        {
            public void Handle(TestEvent eventData, AtataContext context)
            {
                // Method intentionally left empty.
            }
        }

        private sealed class UniversalEventHandler : IEventHandler<object>
        {
            public void Handle(object eventData, AtataContext context)
            {
                // Method intentionally left empty.
            }
        }
    }
}
