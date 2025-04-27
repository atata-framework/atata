namespace Atata.IntegrationTests.Sessions;

public static class AtataSessionsBuilderTests
{
    public sealed class Add
    {
        [Test]
        public void WithoutConfigure()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;

            // Act
            sut.Add<FakeSessionBuilder>();

            // Assert
            sut.Builders.Should().ContainSingle()
                .Which.Should().BeOfType<FakeSessionBuilder>()
                .Which.StartScopes.Should().Be(AtataContextScopes.TestSuite);
        }

        [Test]
        public void WithConfigure()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultNonScopedBuilder().Sessions;

            // Act
            sut.Add<FakeSessionBuilder>(x => x.Name = "some");

            // Assert
            sut.Builders.Should().ContainSingle()
                .Which.Should().BeOfType<FakeSessionBuilder>()
                .Which.Name.Should().Be("some");
        }
    }

    public sealed class Configure_WithConfigureOrThrowMode
    {
        [Test]
        public void WhenNoSuchBuilder()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;

            // Act
            var call = sut.Invoking(x => x.Configure<FakeSessionBuilder>("some", x => x.Mode = AtataSessionMode.Shared));

            // Assert
            call.Should().ThrowExactly<AtataSessionBuilderNotFoundException>()
                .WithMessage("Failed to find FakeSessionBuilder { Name=some }.");
        }

        [Test]
        public void WhenThereIsSuchBuilder()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;
            sut.Add<FakeSessionBuilder>(x => x.Name = "some");

            // Act
            sut.Configure<FakeSessionBuilder>("some", x => x.Mode = AtataSessionMode.Shared);

            // Assert
            sut.Builders.Should().ContainSingle()
                .Which.Should().BeOfType<FakeSessionBuilder>()
                .Which.Should().Match((FakeSessionBuilder x) =>
                    x.Name == "some"
                    && x.StartScopes == AtataContextScopes.TestSuite
                    && x.Mode == AtataSessionMode.Shared);
        }
    }

    public sealed class Configure_WithConfigureIfExistsMode
    {
        [Test]
        public void WhenNoSuchBuilder()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;

            // Act
            sut.Configure<FakeSessionBuilder>("some", x => x.Mode = AtataSessionMode.Shared, ConfigurationMode.ConfigureIfExists);

            // Assert
            sut.Builders.Should().BeEmpty();
        }

        [Test]
        public void WhenThereIsSuchBuilder()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;
            sut.Add<FakeSessionBuilder>(x => x.Name = "some");

            // Act
            sut.Configure<FakeSessionBuilder>("some", x => x.Mode = AtataSessionMode.Shared, ConfigurationMode.ConfigureIfExists);

            // Assert
            sut.Builders.Should().ContainSingle()
                .Which.Should().BeOfType<FakeSessionBuilder>()
                .Which.Should().Match((FakeSessionBuilder x) =>
                    x.Name == "some"
                    && x.StartScopes == AtataContextScopes.TestSuite
                    && x.Mode == AtataSessionMode.Shared);
        }
    }

    public sealed class Configure_WithConfigureOrAddMode
    {
        [Test]
        public void WhenNoSuchBuilder()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;

            // Act
            sut.Configure<FakeSessionBuilder>("some", x => { }, ConfigurationMode.ConfigureOrAdd);

            // Assert
            sut.Builders.Should().ContainSingle()
                .Which.Should().BeOfType<FakeSessionBuilder>()
                .Which.Should().Match((FakeSessionBuilder x) => x.Name == "some" && x.StartScopes == AtataContextScopes.TestSuite);
        }

        [Test]
        public void WhenThereIsSuchBuilder()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;
            sut.Add<FakeSessionBuilder>(x => x.Name = "some");

            // Act
            sut.Configure<FakeSessionBuilder>("some", x => x.Mode = AtataSessionMode.Shared, ConfigurationMode.ConfigureOrAdd);

            // Assert
            sut.Builders.Should().ContainSingle()
                .Which.Should().BeOfType<FakeSessionBuilder>()
                .Which.Should().Match((FakeSessionBuilder x) =>
                    x.Name == "some"
                    && x.StartScopes == AtataContextScopes.TestSuite
                    && x.Mode == AtataSessionMode.Shared);
        }

        [Test]
        public void WhenThereIsBuilderWithDifferentName()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;
            sut.Add<FakeSessionBuilder>(x => x.Name = "some1");

            // Act
            sut.Configure<FakeSessionBuilder>("some2", x => x.Mode = AtataSessionMode.Shared, ConfigurationMode.ConfigureOrAdd);

            // Assert
            sut.Builders.Should().HaveCount(2);

            sut.Builders.Last().Should().BeOfType<FakeSessionBuilder>()
                .Which.Should().Match((FakeSessionBuilder x) =>
                    x.Name == "some2"
                    && x.StartScopes == AtataContextScopes.TestSuite
                    && x.Mode == AtataSessionMode.Shared);
        }
    }

    public sealed class Configure_WithSessionType
    {
        [Test]
        public void WhenThereIsSuchBuilder()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;
            sut.Add<FakeSessionBuilder>(x => x.Name = "some");

            // Act
            sut.Configure(typeof(FakeSession), "some", x => x.Mode = AtataSessionMode.Shared);

            // Assert
            sut.Builders.Should().ContainSingle()
                .Which.Should().BeOfType<FakeSessionBuilder>()
                .Which.Should().Match((FakeSessionBuilder x) =>
                    x.Name == "some"
                    && x.StartScopes == AtataContextScopes.TestSuite
                    && x.Mode == AtataSessionMode.Shared);
        }

        [Test]
        public void WithBaseType_WhenThereIsSuchBuilder()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;
            sut.Add<FakeSessionBuilder>(x => x.Name = "some");

            // Act
            sut.Configure(typeof(AtataSession), "some", x => x.Mode = AtataSessionMode.Shared);

            // Assert
            sut.Builders.Should().ContainSingle()
                .Which.Should().BeOfType<FakeSessionBuilder>()
                .Which.Should().Match((FakeSessionBuilder x) =>
                    x.Name == "some"
                    && x.StartScopes == AtataContextScopes.TestSuite
                    && x.Mode == AtataSessionMode.Shared);
        }

        [Test]
        public void WhenThereIsBuilderWithDifferentName()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultBuilder(AtataContextScope.TestSuite).Sessions;
            sut.Add<FakeSessionBuilder>(x => x.Name = "some1");

            // Act
            var call = sut.Invoking(x => x.Configure(typeof(FakeSession), "some2", x => x.Mode = AtataSessionMode.Shared));

            // Assert
            call.Should().ThrowExactly<AtataSessionBuilderNotFoundException>()
                .WithMessage("Failed to find session builder for FakeSession { Name=some2 }.");
        }
    }

    public sealed class RemoveAll
    {
        [Test]
        public void WhenThereAreBuilders()
        {
            // Arrange
            var sut = AtataContext.CreateDefaultNonScopedBuilder().Sessions;
            sut.Add<FakeSessionBuilder>();
            sut.Add<FakeSessionBuilder2>();
            sut.Add<FakeSessionBuilder>(x => x.Name = "some");

            // Act
            sut.RemoveAll<FakeSessionBuilder>();

            // Assert
            sut.Builders.Should().ContainSingle()
                .Which.Should().BeOfType<FakeSessionBuilder2>();
        }
    }

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
            session.IsShareable.Should().BeTrue();
            session.IsBorrowed.Should().BeTrue();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEquivalentTo([session]);
            parentContext.Sessions.GetAllIncludingPooled().Should().BeEmpty();
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
            session.IsShareable.Should().BeFalse();
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
            session.IsShareable.Should().BeFalse();
            session.Context.Should().Be(context);
            session.OwnerContext.Should().Be(parentContext);
            context.Sessions.Should().BeEquivalentTo([session]);
            parentContext.Sessions.GetAllIncludingPooled().Should().BeEmpty();
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
