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
            session.IsShareable.Should().BeTrue();
            session.IsTakenFromPool.Should().BeFalse();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEquivalentTo([session]);
            parentContext.Sessions.GetAllIncludingPooled().Should().BeEmpty();
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
            session.IsShareable.Should().BeTrue();
            session.Mode.Should().Be(AtataSessionMode.Shared);
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

        [Test]
        public async Task WhenPoolInGrandparentContextAndSharedByParent_ThenDisposeContexts()
        {
            // Arrange
            await using var grandparentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseMode(AtataSessionMode.Pool)
                    .UseName("some"))
                .BuildAsync();

            await using var parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(grandparentContext)
                .Sessions.TakeFromPool<FakeSession>(x => x
                    .UseSharedMode(true)
                    .UseName("some"))
                .BuildAsync();

            await using var context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            // Act
            var session = await context.Sessions.BorrowAsync<FakeSession>("some");

            // Assert
            session.IsTakenFromPool.Should().BeTrue();
            session.IsShareable.Should().BeTrue();
            session.IsBorrowed.Should().BeTrue();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(grandparentContext);
            context.Sessions.Should().BeEquivalentTo([session]);
            parentContext.Sessions.GetAllIncludingPooled().Should().BeEmpty();
            grandparentContext.Sessions.GetAllIncludingPooled().Should().BeEmpty();

            // Act
            await context.DisposeAsync();

            // Assert
            session.IsTakenFromPool.Should().BeTrue();
            session.IsShareable.Should().BeTrue();
            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().Be(parentContext);
            session.OwnerContext.Should().Be(grandparentContext);
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEquivalentTo([session]);
            grandparentContext.Sessions.GetAllIncludingPooled().Should().BeEmpty();

            // Act
            await parentContext.DisposeAsync();

            // Assert
            session.IsTakenFromPool.Should().BeFalse();
            session.IsShareable.Should().BeFalse();
            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().Be(grandparentContext);
            session.OwnerContext.Should().Be(grandparentContext);
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEmpty();
            grandparentContext.Sessions.Should().BeEmpty();
            grandparentContext.Sessions.GetAllIncludingPooled().Should().BeEquivalentTo([session]);
        }

        [Test]
        public async Task WhenPoolInGrandparentContextAndSharedByParent_ThenReturnToSessionSources()
        {
            // Arrange
            await using var grandparentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseMode(AtataSessionMode.Pool)
                    .UseName("some"))
                .BuildAsync();

            await using var parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(grandparentContext)
                .Sessions.TakeFromPool<FakeSession>(x => x
                    .UseSharedMode(true)
                    .UseName("some"))
                .BuildAsync();

            await using var context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            // Act
            var session = await context.Sessions.BorrowAsync<FakeSession>("some");

            // Assert
            session.IsTakenFromPool.Should().BeTrue();
            session.IsShareable.Should().BeTrue();
            session.IsBorrowed.Should().BeTrue();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(grandparentContext);
            context.Sessions.Should().BeEquivalentTo([session]);
            parentContext.Sessions.GetAllIncludingPooled().Should().BeEmpty();
            grandparentContext.Sessions.GetAllIncludingPooled().Should().BeEmpty();

            // Act
            session.ReturnToSessionSource();

            // Assert
            session.IsTakenFromPool.Should().BeTrue();
            session.IsShareable.Should().BeTrue();
            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().Be(parentContext);
            session.OwnerContext.Should().Be(grandparentContext);
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEquivalentTo([session]);
            grandparentContext.Sessions.GetAllIncludingPooled().Should().BeEmpty();

            // Act
            session.ReturnToSessionSource();

            // Assert
            session.IsTakenFromPool.Should().BeFalse();
            session.IsShareable.Should().BeFalse();
            session.IsBorrowed.Should().BeFalse();
            session.Context.Should().Be(grandparentContext);
            session.OwnerContext.Should().Be(grandparentContext);
            context.Sessions.Should().BeEmpty();
            parentContext.Sessions.Should().BeEmpty();
            grandparentContext.Sessions.Should().BeEmpty();
            grandparentContext.Sessions.GetAllIncludingPooled().Should().BeEquivalentTo([session]);
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
            parentContext.Sessions.GetAllIncludingPooled().Should().BeEmpty();
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
            session.Mode.Should().Be(AtataSessionMode.Pool);
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
            parentContext.Sessions.Should().BeEmpty();
            parentContext.Sessions.GetAllIncludingPooled().Should().BeEquivalentTo([session]);
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
            parentContext.Sessions.Should().BeEmpty();
            parentContext.Sessions.GetAllIncludingPooled().Should().BeEquivalentTo([session]);
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

    public sealed class GetRecursively
    {
        [Test]
        public async Task WhenThereIsSessionInContext()
        {
            // Arrange
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>()
                .BuildAsync();

            // Act
            var session = context.Sessions.GetRecursively<FakeSession>();

            // Assert
            session.Context.Should().Be(context);
        }

        [Test]
        public async Task WhenThereIsSessionInParentContext()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseName("some"))
                .BuildAsync();

            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            // Act
            var session = context.Sessions.GetRecursively<FakeSession>();

            // Assert
            session.Context.Should().Be(parentContext);
        }

        [Test]
        public async Task WhenThereIsNoSession()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .BuildAsync();

            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            // Act
            var call = context.Invoking(x => x.Sessions.GetRecursively<FakeSession>());

            // Assert
            call.Should().ThrowExactly<AtataSessionNotFoundException>()
                .WithMessage("Failed to find FakeSession in AtataContext { * } and ancestors.");
        }
    }

    public sealed class GetRecursively_WithName
    {
        [Test]
        public async Task WhenThereIsSessionInParentContext()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseName("some"))
                .BuildAsync();

            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            // Act
            var session = context.Sessions.GetRecursively<FakeSession>("some");

            // Assert
            session.Context.Should().Be(parentContext);
        }

        [Test]
        public async Task WhenThereIsSessionInParentContext_ButWithDifferentName()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>(x => x
                    .UseName("some"))
                .BuildAsync();

            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            // Act
            var call = context.Invoking(x => x.Sessions.GetRecursively<FakeSession>("some2"));

            // Assert
            call.Should().ThrowExactly<AtataSessionNotFoundException>()
                .WithMessage("Failed to find FakeSession { Name=some2 } in AtataContext { * } and ancestors. There was 1 session of such type, but none with such name.");
        }

        [Test]
        public async Task WhenThereIsNoSession()
        {
            // Arrange
            await using AtataContext parentContext = await AtataContext.CreateDefaultNonScopedBuilder()
                .BuildAsync();

            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .UseParentContext(parentContext)
                .BuildAsync();

            // Act
            var call = context.Invoking(x => x.Sessions.GetRecursively<FakeSession>("some"));

            // Assert
            call.Should().ThrowExactly<AtataSessionNotFoundException>()
                .WithMessage("Failed to find FakeSession { Name=some } in AtataContext { * } and ancestors. There were 0 sessions of such type.");
        }
    }
}
