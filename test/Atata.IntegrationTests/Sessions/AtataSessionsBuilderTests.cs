namespace Atata.IntegrationTests.Sessions;

public static class AtataSessionsBuilderTests
{
    public sealed class Borrow
    {
        [Test]
        public async Task WhenNoSessionToBorrow()
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Borrow<FakeSession>();

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionToBorrowAsync(contextBuilder);
        }

        [Test]
        public async Task WhenSuchSessionInSameContext()
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.Mode = AtataSessionMode.Shared)
                .Sessions.Borrow<FakeSession>();

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionToBorrowAsync(contextBuilder);
        }

        [Test]
        public async Task WhenSharedSessionHasNameButRequestDoesNot()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseName("some")
                    .UseMode(AtataSessionMode.Shared))
                .BuildAsync();

            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .Sessions.Borrow<FakeSession>();

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionToBorrowAsync(contextBuilder);
        }

        [Test]
        public async Task WhenSharedSessionDoesNotHaveNameButRequestDoes()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseMode(AtataSessionMode.Shared))
                .BuildAsync();

            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .Sessions.Borrow<FakeSession>(x => x.Name = "some");

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionToBorrowAsync(contextBuilder);
        }

        [Test]
        public async Task WhenSharedSessionAndRequestNamesDiffer()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseName("some1")
                    .UseMode(AtataSessionMode.Shared))
                .BuildAsync();

            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .Sessions.Borrow<FakeSession>(x => x.Name = "some2");

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionToBorrowAsync(contextBuilder);
        }

        [Test]
        public async Task WhenSharedSessionAndRequestTypesDiffer()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseName("some")
                    .UseMode(AtataSessionMode.Shared))
                .BuildAsync();

            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .Sessions.Borrow<FakeSession2>(x => x.Name = "some");

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionToBorrowAsync(contextBuilder);
        }

        [Test]
        public async Task WhenSharedSessionInParentContext()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.UseMode(AtataSessionMode.Shared))
                .BuildAsync();

            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .Sessions.Borrow<FakeSession>();

            // Act
            await using AtataContext context = await contextBuilder.BuildAsync();

            // Assert
            var session = context.Sessions.Get<FakeSession>();

            session.Mode.Should().Be(AtataSessionMode.Shared);
            session.IsBorrowed.Should().BeTrue();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEquivalentTo([session]);
            parentContext.Sessions.Should().BeEquivalentTo([session]);
        }

        private static async Task AssertContextBuildFailureBecauseOfNotFoundSessionToBorrowAsync(AtataContextBuilder contextBuilder)
        {
            // Act
            var call = contextBuilder.Invoking(x => x.BuildAsync());

            // Assert
            await call.Should().ThrowExactlyAsync<AtataSessionNotFoundException>()
                .WithMessage("Failed to find FakeSession* to borrow for AtataContext { * }.");
        }
    }

    public sealed class TakeFromPool
    {
        [Test]
        public async Task WhenNoPool()
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.TakeFromPool<FakeSession>(x => x.Name = "some");

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionPoolAsync(contextBuilder);
        }

        [Test]
        public async Task WhenPoolHasNameButRequestDoesNot()
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseName("some")
                    .UseMode(AtataSessionMode.Pool))
                .Sessions.TakeFromPool<FakeSession>();

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionPoolAsync(contextBuilder);
        }

        [Test]
        public async Task WhenPoolDoesNotHaveNameButRequestDoes()
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseMode(AtataSessionMode.Pool))
                .Sessions.TakeFromPool<FakeSession>(x => x.Name = "some");

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionPoolAsync(contextBuilder);
        }

        [Test]
        public async Task WhenPoolAndRequestNamesDiffer()
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseName("some1")
                    .UseMode(AtataSessionMode.Pool))
                .Sessions.TakeFromPool<FakeSession>(x => x.Name = "some2");

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionPoolAsync(contextBuilder);
        }

        [Test]
        public async Task WhenPoolAndRequestTypesDiffer()
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseName("some")
                    .UseMode(AtataSessionMode.Pool))
                .Sessions.TakeFromPool<FakeSession2>(x => x.Name = "some");

            // Act // Assert
            await AssertContextBuildFailureBecauseOfNotFoundSessionPoolAsync(contextBuilder);
        }

        [Test]
        public async Task WhenPoolInSameContext()
        {
            // Arrange
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseName("some")
                    .UseMode(AtataSessionMode.Pool))
                .Sessions.TakeFromPool<FakeSession>(x => x.Name = "some");

            // Act
            await using AtataContext context = await contextBuilder.BuildAsync();

            // Assert
            var session = context.Sessions.Get<FakeSession>("some");

            session.Mode.Should().Be(AtataSessionMode.Pool);
            session.IsTakenFromPool.Should().BeTrue();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(context);
            context.Sessions.Should().BeEquivalentTo([session]);
        }

        [Test]
        public async Task WhenPoolInParentContext()
        {
            // Arrange
            await using var parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.UseMode(AtataSessionMode.Pool))
                .BuildAsync();

            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .Sessions.TakeFromPool<FakeSession>();

            // Act
            await using AtataContext context = await contextBuilder.BuildAsync();

            // Assert
            var session = context.Sessions.Get<FakeSession>();

            session.Mode.Should().Be(AtataSessionMode.Pool);
            session.IsTakenFromPool.Should().BeTrue();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEquivalentTo([session]);
            parentContext.Sessions.Should().BeEquivalentTo([session]);
        }

        private static async Task AssertContextBuildFailureBecauseOfNotFoundSessionPoolAsync(AtataContextBuilder contextBuilder)
        {
            // Act
            var call = contextBuilder.Invoking(x => x.BuildAsync());

            // Assert
            await call.Should().ThrowExactlyAsync<AtataSessionPoolNotFoundException>()
                .WithMessage("Failed to find FakeSession* pool to take session for AtataContext { * }.");
        }
    }
}
