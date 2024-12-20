namespace Atata.IntegrationTests.Sessions;

public static class AtataSessionCollectionTests
{
    public sealed class BorrowAsync
    {
        [Test]
        public async Task WhenNoSessionToBorrow()
        {
            // Arrange
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();

            // Act
            var call = context.Sessions.Invoking(x => x.BorrowAsync<FakeSession>().AsTask());

            // Assert
            await call.Should().ThrowExactlyAsync<AtataSessionNotFoundException>()
                .WithMessage("Failed to find FakeSession to borrow for AtataContext { * }.");
        }

        [Test]
        public async Task WhenSharedSessionInParentContext()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.UseMode(AtataSessionMode.Shared))
                .BuildAsync();

            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            // Act
            var session = await context.Sessions.BorrowAsync<FakeSession>();

            // Assert
            session.IsBorrowed.Should().BeTrue();
            session.IsTakenFromPool.Should().BeFalse();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEquivalentTo([session]);
            parentContext.Sessions.Should().BeEquivalentTo([session]);
        }

        [Test]
        public async Task WhenSharedSessionInParentContext_ThenDisposeSession()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.UseMode(AtataSessionMode.Shared))
                .BuildAsync();

            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            var session = await context.Sessions.BorrowAsync<FakeSession>();

            // Act
            await session.DisposeAsync();

            // Assert
            session.IsActive.Should().BeFalse();
            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().BeNull();
            session.OwnerContext.Should().BeNull();
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task WhenSharedSessionInParentContext_ThenDisposeContext()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.UseMode(AtataSessionMode.Shared))
                .BuildAsync();

            AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            var session = await context.Sessions.BorrowAsync<FakeSession>();

            // Act
            await context.DisposeAsync();

            // Assert
            session.IsActive.Should().BeTrue();
            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().Be(parentContext);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEquivalentTo([session]);
        }

        [Test]
        public async Task WhenSharedSessionInParentContext_ThenReturnToSessionSource()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.UseMode(AtataSessionMode.Shared))
                .BuildAsync();

            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            var session = await context.Sessions.BorrowAsync<FakeSession>();

            // Act
            session.ReturnToSessionSource();

            // Assert
            session.IsActive.Should().BeTrue();
            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().Be(parentContext);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEquivalentTo([session]);
        }
    }

    public sealed class TakeFromPoolAsync
    {
        [Test]
        public async Task WhenNoPool()
        {
            // Arrange
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();

            // Act
            var call = context.Sessions.Invoking(x => x.TakeFromPoolAsync<FakeSession>().AsTask());

            // Assert
            await call.Should().ThrowExactlyAsync<AtataSessionPoolNotFoundException>()
                .WithMessage("Failed to find FakeSession pool to take session for AtataContext { * }.");
        }

        [Test]
        public async Task WhenPoolInParentContext()
        {
            // Arrange
            await using var parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseMode(AtataSessionMode.Pool)
                    .UseName("some"))
                .BuildAsync();

            await using var context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            // Act
            var session = await context.Sessions.TakeFromPoolAsync<FakeSession>("some");

            // Assert
            session.IsTakenFromPool.Should().BeTrue();
            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEquivalentTo([session]);
            parentContext.Sessions.Should().BeEquivalentTo([session]);
        }

        [Test]
        public async Task WhenPoolInParentContext_ThenDisposeSession()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.UseMode(AtataSessionMode.Pool))
                .BuildAsync();

            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            var session = await context.Sessions.TakeFromPoolAsync<FakeSession>();

            // Act
            await session.DisposeAsync();

            // Assert
            session.IsActive.Should().BeFalse();
            session.IsTakenFromPool.Should().BeFalse();
            session.Context.Should().BeNull();
            session.OwnerContext.Should().BeNull();
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task WhenPoolInParentContext_ThenDisposeContext()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.UseMode(AtataSessionMode.Pool))
                .BuildAsync();

            AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            var session = await context.Sessions.TakeFromPoolAsync<FakeSession>();

            // Act
            await context.DisposeAsync();

            // Assert
            session.IsActive.Should().BeTrue();
            session.IsTakenFromPool.Should().BeFalse();
            session.Context.Should().Be(parentContext);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEquivalentTo([session]);
        }

        [Test]
        public async Task WhenPoolInParentContext_ThenReturnToSessionSource()
        {
            // Arrange
            await using var parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.UseMode(AtataSessionMode.Pool))
                .BuildAsync();

            await using var context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            var session = await context.Sessions.TakeFromPoolAsync<FakeSession>();

            // Act
            session.ReturnToSessionSource();

            // Assert
            session.IsActive.Should().BeTrue();
            session.IsTakenFromPool.Should().BeFalse();
            session.Context.Should().Be(parentContext);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEquivalentTo([session]);
        }
    }

    public sealed class BuildAsync
    {
        [Test]
        public async Task WhenNoBuilder()
        {
            // Arrange
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();

            // Act
            var call = context.Sessions.Invoking(x => x.BuildAsync<FakeSession>());

            // Assert
            await call.Should().ThrowExactlyAsync<AtataSessionBuilderNotFoundException>()
                .WithMessage("Failed to find session builder for FakeSession in AtataContext { * }.");
        }

        [Test]
        public async Task WhenThereIsBuilderWithAnotherName()
        {
            // Arrange
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseStartScopes(AtataSessionStartScopes.None)
                    .UseName("some1"))
                .BuildAsync();

            // Act
            var call = context.Sessions.Invoking(x => x.BuildAsync<FakeSession>("some2"));

            // Assert
            await call.Should().ThrowExactlyAsync<AtataSessionBuilderNotFoundException>()
                .WithMessage("Failed to find session builder for FakeSession { * } in AtataContext { * }.");
        }

        [Test]
        public async Task WithoutName_WhenThereIsBuilder()
        {
            // Arrange
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x.UseStartScopes(AtataSessionStartScopes.None))
                .BuildAsync();

            // Act
            var session = await context.Sessions.BuildAsync<FakeSession>();

            // Assert
            session.IsBorrowed.Should().BeFalse();
            session.IsTakenFromPool.Should().BeFalse();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(context);
            context.Sessions.Should().BeEquivalentTo([session]);
        }

        [Test]
        public async Task WithName_WhenThereIsBuilder()
        {
            // Arrange
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseStartScopes(AtataSessionStartScopes.None)
                    .UseName("some"))
                .BuildAsync();

            // Act
            var session = await context.Sessions.BuildAsync<FakeSession>("some");

            // Assert
            session.IsBorrowed.Should().BeFalse();
            session.IsTakenFromPool.Should().BeFalse();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(context);
            context.Sessions.Should().BeEquivalentTo([session]);
        }

        [Test]
        public async Task WithoutName_WhenThereIsBuilderWithBuiltSession()
        {
            // Arrange
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>()
                .BuildAsync();

            // Act
            var session = await context.Sessions.BuildAsync<FakeSession>();

            // Assert
            session.IsBorrowed.Should().BeFalse();
            session.IsTakenFromPool.Should().BeFalse();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(context);
            context.Sessions.Should().HaveCount(2);
            context.Sessions[1].Should().BeEquivalentTo(session);
        }
    }
}
