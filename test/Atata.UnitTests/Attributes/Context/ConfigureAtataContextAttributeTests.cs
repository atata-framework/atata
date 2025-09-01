namespace Atata.UnitTests.Attributes.Context;

public static class ConfigureAtataContextAttributeTests
{
    public sealed class ConfigureAtataContext
    {
        private AtataContextBuilder _builder = null!;

        [SetUp]
        public void SetUp() =>
            _builder = AtataContext.CreateDefaultNonScopedBuilder();

        [Test]
        public void WithNullTestSuite_WhenTargetTypeIsNull()
        {
            // Arrange
            ConfigureAtataContextAttribute attribute = new(nameof(ConfigureViaThisStaticMethod));

            // Act // Assert
            Assert.That(
                () => attribute.ConfigureAtataContext(_builder, null),
                Throws.InvalidOperationException);
        }

        [Test]
        public void WithTestSuite_WhenMethodNameDoesNotExist()
        {
            // Arrange
            ConfigureAtataContextAttribute attribute = new("Missing");

            // Act // Assert
            Assert.That(
                () => attribute.ConfigureAtataContext(_builder, this),
                Throws.TypeOf<MissingMethodException>());
        }

        [Test]
        public void WithTestSuite_WhenTargetTypeIsNullAndMethodIsStatic()
        {
            // Arrange
            ConfigureAtataContextAttribute attribute = new(nameof(ConfigureViaThisStaticMethod));

            // Act
            attribute.ConfigureAtataContext(_builder, this);

            // Assert
            Assert.That(_builder.State[nameof(ConfigureViaThisStaticMethod)], Is.True);
        }

        [Test]
        public void WithTestSuite_WhenTargetTypeIsNullAndMethodIsInstance()
        {
            // Arrange
            ConfigureAtataContextAttribute attribute = new(nameof(ConfigureViaThisInstanceMethod));

            // Act
            attribute.ConfigureAtataContext(_builder, this);

            // Assert
            Assert.That(_builder.State[nameof(ConfigureViaThisInstanceMethod)], Is.True);
        }

        [Test]
        public void WithTestSuite_WhenTargetTypeIsNotNull()
        {
            // Arrange
            ConfigureAtataContextAttribute attribute = new(typeof(OtherClass), nameof(OtherClass.ConfigureViaOtherStaticMethod));

            // Act
            attribute.ConfigureAtataContext(_builder, this);

            // Assert
            Assert.That(_builder.State[nameof(OtherClass.ConfigureViaOtherStaticMethod)], Is.True);
        }

        [Test]
        public void WithoutTestSuite_WhenTargetTypeIsNotNull()
        {
            // Arrange
            ConfigureAtataContextAttribute attribute = new(typeof(OtherClass), nameof(OtherClass.ConfigureViaOtherInstanceMethod));

            // Act
            attribute.ConfigureAtataContext(_builder, this);

            // Assert
            Assert.That(_builder.State[nameof(OtherClass.ConfigureViaOtherInstanceMethod)], Is.True);
        }

        private static void ConfigureViaThisStaticMethod(AtataContextBuilder builder) =>
            builder.UseState(nameof(ConfigureViaThisStaticMethod), true);

        private void ConfigureViaThisInstanceMethod(AtataContextBuilder builder) =>
            builder.UseState(nameof(ConfigureViaThisInstanceMethod), true);

        public sealed class OtherClass
        {
            public static void ConfigureViaOtherStaticMethod(AtataContextBuilder builder) =>
                builder.UseState(nameof(ConfigureViaOtherStaticMethod), true);

            public void ConfigureViaOtherInstanceMethod(AtataContextBuilder builder) =>
                builder.UseState(nameof(ConfigureViaOtherInstanceMethod), true);
        }
    }
}
