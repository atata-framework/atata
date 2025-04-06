namespace Atata.UnitTests.Sessions;

public static class AtataSessionEventSubscriptionsBuilderTests
{
    public sealed class Add
    {
        private Subject<AtataSessionEventSubscriptionsBuilder<AtataContextBuilder>> _sut = null!;

        [SetUp]
        public void SetUp() =>
            _sut = new AtataSessionEventSubscriptionsBuilder<AtataContextBuilder>(AtataContext.CreateDefaultNonScopedBuilder())
                .ToSutSubject();

        [Test]
        public void NullAsAction() =>
            _sut.Invoking(x => x.Add<TestEvent>((null as Action)!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public void Action() =>
            _sut.Act(x => x.Add<TestEvent>(StubMethod))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler != null);

        [Test]
        public void AsyncAction() =>
            _sut.Act(x => x.Add<TestEvent>(StubMethodAsync))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler != null);

        [Test]
        public void ActionWith1GenericParameter() =>
            _sut.Act(x => x.Add<TestEvent>(_ => StubMethod()))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler != null);

        [Test]
        public void AsyncActionWith1GenericParameter() =>
            _sut.Act(x => x.Add<TestEvent>((_, ct) => StubMethodAsync(ct)))
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
        public void AsyncActionWith2GenericParameters() =>
            _sut.Act(x => x.Add<TestEvent>((_, _, ct) => StubMethodAsync(ct)))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler != null);

        [Test]
        public void TwoGenericParameters_WithInvalidEventHandlerType() =>
            _sut.Invoking(x => x.Add<TestEvent, TestEvent>())
                .Should.ThrowExactly<ArgumentException>(
                    $"'eventHandlerType' of {typeof(TestEvent).FullName} type doesn't implement Atata.IEventHandler`1[*] or Atata.IAsyncEventHandler`1[*]. (Parameter 'eventHandlerType')");

        [Test]
        public void TwoGenericParameters_WithSyncEventHandlerType() =>
            _sut.Act(x => x.Add<TestEvent, TestEventHandler>())
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

        [Test]
        public void TwoGenericParameters_WithAsyncEventHandlerType() =>
            _sut.Act(x => x.Add<TestEvent, TestAsyncEventHandler>())
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is TestAsyncEventHandler);

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
        public void AsyncEventHandler()
        {
            var eventHandler = new TestAsyncEventHandler();

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
            var eventHandler3 = new TestAsyncEventHandler();

            _sut.Act(x => x.Add(eventHandler1))
                .Act(x => x.Add<TestEvent>(eventHandler2))
                .Act(x => x.Add(eventHandler3))
                .ResultOf(x => x.Items)
                    .Should.ConsistSequentiallyOf(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler == eventHandler1,
                        x => x.EventType == typeof(TestEvent) && x.EventHandler == eventHandler2,
                        x => x.EventType == typeof(TestEvent) && x.EventHandler == eventHandler3);
        }

        [Test]
        public void EventHandlerType_WithSyncEventHandlerType() =>
            _sut.Act(x => x.Add(typeof(TestEventHandler)))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

        [Test]
        public void EventHandlerType_WithAsyncEventHandlerType() =>
            _sut.Act(x => x.Add(typeof(TestAsyncEventHandler)))
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is TestAsyncEventHandler);

        [Test]
        public void EventHandlerType_WithInvalidEventHandlerType() =>
            _sut.Invoking(x => x.Add(typeof(AtataSessionEventSubscriptionsBuilderTests)))
                .Should.ThrowExactly<ArgumentException>();

        [Test]
        public void EventTypeAndEventHandlerType_WithExactSyncEventHandlerType() =>
            _sut.Act(x => x.Add<TestEvent, TestEventHandler>())
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

        [Test]
        public void EventTypeAndEventHandlerType_WithExactAsyncEventHandlerType() =>
            _sut.Act(x => x.Add<TestEvent, TestAsyncEventHandler>())
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is TestAsyncEventHandler);

        [Test]
        public void EventTypeAndEventHandlerType_WithBaseSyncEventHandlerType() =>
            _sut.Act(x => x.Add<TestEvent, UniversalEventHandler>())
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is UniversalEventHandler);

        [Test]
        public void EventTypeAndEventHandlerType_WithBaseAsyncEventHandlerType() =>
            _sut.Act(x => x.Add<TestEvent, UniversalAsyncEventHandler>())
                .ResultOf(x => x.Items)
                    .Should.ConsistOfSingle(
                        x => x.EventType == typeof(TestEvent) && x.EventHandler is UniversalAsyncEventHandler);

        [Test]
        public void EventTypeAndEventHandlerType_WithInvalidEventHandlerType() =>
            _sut.Invoking(x => x.Add(typeof(TestEvent), typeof(AtataSessionEventSubscriptionsBuilderTests)))
                .Should.ThrowExactly<ArgumentException>();

        private static void StubMethod()
        {
            // Method intentionally left empty.
        }

        private static Task StubMethodAsync(CancellationToken cancellationToken) =>
            Task.CompletedTask;

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

        private sealed class TestAsyncEventHandler : IAsyncEventHandler<TestEvent>
        {
            public Task HandleAsync(TestEvent eventData, AtataContext context, CancellationToken cancellationToken) =>
                Task.CompletedTask;
        }

        private sealed class UniversalEventHandler : IEventHandler<object>
        {
            public void Handle(object eventData, AtataContext context)
            {
                // Method intentionally left empty.
            }
        }

        private sealed class UniversalAsyncEventHandler : IAsyncEventHandler<object>
        {
            public Task HandleAsync(object eventData, AtataContext context, CancellationToken cancellationToken) =>
                Task.CompletedTask;
        }
    }

    public sealed class RemoveAll
    {
        private Subject<AtataSessionEventSubscriptionsBuilder<AtataContextBuilder>> _sut = null!;

        [SetUp]
        public void SetUp() =>
            _sut = new AtataSessionEventSubscriptionsBuilder<AtataContextBuilder>(AtataContext.CreateDefaultNonScopedBuilder())
                .ToSutSubject();

        [Test]
        public void WithNull() =>
            _sut.Invoking(x => x.RemoveAll(null!))
                .Should.Throw<ArgumentNullException>();

        [Test]
        public void WhenThereIsSuchItem() =>
            _sut.Act(x => x.Add<TestEvent>(StubMethod))
                .Act(x => x.RemoveAll(x => x.EventType == typeof(TestEvent)))
                .ValueOf(x => x.Items).Should.BeEmpty();

        [Test]
        public void WhenThereIsNoSuchItem() =>
            _sut.Act(x => x.Add<TestEvent>(StubMethod))
                .Act(x => x.RemoveAll(x => x.EventType == typeof(AtataContextInitCompletedEvent)))
                .ValueOf(x => x.Items).Should.ContainSingle();

        private static void StubMethod()
        {
            // Method intentionally left empty.
        }

        public sealed class TestEvent
        {
        }
    }
}
