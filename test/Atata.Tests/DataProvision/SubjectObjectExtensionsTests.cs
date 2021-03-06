﻿using System;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests.DataProvision
{
    [TestFixture]
    public static class SubjectObjectExtensionsTests
    {
        [TestFixture]
        public static class ToSubject
        {
            [Test]
            public static void WithName() =>
                42.ToSubject("some name")
                    .ProviderName.Should().Be("some name");

            [Test]
            public static void DefaultName() =>
                42.ToSubject()
                    .ProviderName.Should().Be("subject");

            [Test]
            public static void ReferenceType() =>
                new Uri("/", UriKind.Relative).ToSubject()
                    .Value.Should().NotBeNull();

            [Test]
            public static void ValueType() =>
                42.ToSubject()
                    .Value.Should().Be(42);

            [Test]
            public static void NullValue() =>
                (null as Uri).ToSubject()
                    .Value.Should().BeNull();
        }
    }
}
