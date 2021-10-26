using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class UIComponentTests : UITestFixture
    {
        [Test]
        public void UIComponent_ComponentLocation()
        {
            Go.To<InputPage>().
                TextInput.ComponentLocation.X.Should.BeGreater(10).
                TextInput.ComponentLocation.Y.Should.BeInRange(10, 1000).
                TextInput.ComponentLocation.Y.Get(out int y).
                TextInput.ComponentLocation.Y.Should.Equal(y).

                TextInput.ComponentLocation.Get(out var location).
                TextInput.ComponentLocation.Should.Equal(location);
        }

        [Test]
        public void UIComponent_ComponentSize()
        {
            Go.To<InputPage>().
                TextInput.ComponentSize.Width.Should.BeGreater(20).
                TextInput.ComponentSize.Height.Should.BeInRange(10, 100).
                TextInput.ComponentSize.Height.Get(out int height).
                TextInput.ComponentSize.Height.Should.Equal(height).

                TextInput.ComponentSize.Get(out var size).
                TextInput.ComponentSize.Should.Equal(size);
        }

        [Test]
        public void UIComponent_IsPresent()
        {
            var page = Go.To<ContentPage>();

            using (StopwatchAsserter.WithinSeconds(0))
                page.VisibleDiv.IsPresent.Value.Should().BeTrue();

            using (StopwatchAsserter.WithinSeconds(0))
                page.VisibleDiv.IsPresent.Should.BeTrue();

            using (StopwatchAsserter.WithinSeconds(0))
                page.VisibleDiv.Should.Exist();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDiv.IsPresent.Value.Should().BeTrue();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDiv.IsPresent.Should.BeTrue();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDiv.Should.Exist();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDivWithVisibleVisibility.IsPresent.Value.Should().BeFalse();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDivWithVisibleVisibility.IsPresent.Should.BeFalse();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDivWithVisibleVisibility.Should.Not.Exist();
        }

        [Test]
        public void UIComponent_IsPresent_Should_Fail()
        {
            var page = Go.To<ContentPage>();

            using (StopwatchAsserter.WithinSeconds(2))
                Assert.Throws<AssertionException>(() =>
                    page.VisibleDiv.IsPresent.Should.Within(2).BeFalse());

            using (StopwatchAsserter.WithinSeconds(2))
                Assert.Throws<AssertionException>(() =>
                    page.HiddenDiv.IsPresent.Should.Within(2).BeFalse());

            using (StopwatchAsserter.WithinSeconds(2))
                Assert.Throws<AssertionException>(() =>
                    page.HiddenDivWithVisibleVisibility.IsPresent.Should.Within(2).BeTrue());
        }

        [Test]
        public void UIComponent_IsVisible()
        {
            var page = Go.To<ContentPage>();

            using (StopwatchAsserter.WithinSeconds(0))
                page.VisibleDiv.IsVisible.Value.Should().BeTrue();

            using (StopwatchAsserter.WithinSeconds(0))
                page.VisibleDiv.IsVisible.Should.BeTrue();

            using (StopwatchAsserter.WithinSeconds(0))
                page.VisibleDiv.Should.BeVisible();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDiv.IsVisible.Value.Should().BeFalse();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDiv.IsVisible.Should.BeFalse();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDiv.Should.BeHidden();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDivWithVisibleVisibility.IsVisible.Value.Should().BeFalse();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDivWithVisibleVisibility.IsVisible.Should.BeFalse();

            using (StopwatchAsserter.WithinSeconds(0))
                page.HiddenDivWithVisibleVisibility.Should.BeHidden();
        }

        [Test]
        public void UIComponent_IsVisible_Should_Fail()
        {
            var page = Go.To<ContentPage>();

            using (StopwatchAsserter.WithinSeconds(2, 1.5))
                Assert.Throws<AssertionException>(() =>
                    page.VisibleDiv.IsVisible.Should.Within(2).BeFalse());

            using (StopwatchAsserter.WithinSeconds(2, 1.5))
                Assert.Throws<AssertionException>(() =>
                    page.HiddenDiv.IsVisible.Should.Within(2).BeTrue());

            using (StopwatchAsserter.WithinSeconds(2, 1.5))
                Assert.Throws<AssertionException>(() =>
                    page.HiddenDivWithVisibleVisibility.IsVisible.Should.Within(2).BeTrue());
        }

        [Test]
        public void UIComponent_Wait_Until_Visible()
        {
            Go.To<WaitingOnInitPage>().
                ContentBlock.Wait(Until.Visible).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void UIComponent_Wait_Until_Hidden()
        {
            Go.To<WaitingOnInitPage>().
                LoadingBlock.Wait(Until.Hidden).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void UIComponent_Wait_Until_MissingOrHidden()
        {
            Go.To<WaitingOnInitPage>().
                LoadingBlock.Wait(Until.MissingOrHidden).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void UIComponent_Wait_Until_VisibleThenHidden()
        {
            Go.To<WaitingOnInitPage>().
                LoadingBlock.Wait(Until.VisibleThenHidden).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void UIComponent_Wait_Until_HiddenThenVisible()
        {
            Go.To<WaitingOnInitPage>().
                ContentBlock.Wait(Until.HiddenThenVisible).
                VerifyContentBlockIsLoaded();
        }

        [Test]
        public void UIComponent_Wait_WithTimeout()
        {
            var page = Go.To<WaitingOnInitPage>();

            Assert.Throws<NoSuchElementException>(() =>
                page.ContentBlock.Wait(Until.Visible, new WaitOptions(1)));
        }

        [Test]
        public void UIComponent_Wait_WithoutThrow()
        {
            Go.To<WaitingOnInitPage>().
                ContentBlock.Wait(Until.Visible, new WaitOptions(1) { ThrowOnPresenceFailure = false }).
                LoadingBlock.Should.AtOnce.BeVisible();
        }

        public class Find : UITestFixture
        {
            private const string TestName = "some control";

            [Test]
            public void WithoutAttributes_WithName() =>
                Go.To<InputPage>()
                    .Find<TextInput<InputPage>>("Enum Text Input")
                        .Attributes.Id.Should.Equal("enum-text-input");

            [Test]
            public void WithAttributes_WithoutName() =>
                Go.To<InputPage>()
                    .Find<TextInput<InputPage>>(new FindByIdAttribute("enum-text-input"))
                        .Attributes.Id.Should.Equal("enum-text-input");

            [Test]
            public void WithAttributes_WithName() =>
                Go.To<InputPage>()
                    .Find<TextInput<InputPage>>("Email Input", new ControlDefinitionAttribute("input[@type='email']"))
                        .Attributes.Type.Should.Equal("email");

            [Test]
            public void Name_WhenNameIsNotSet_WithFindByIdAttribute() =>
                Go.To<InputPage>()
                    .Find<TextInput<InputPage>>(new FindByIdAttribute("text-input"))
                    .ToSutSubject()
                        .ValueOf(x => x.ComponentName).Should.Equal("FindById:text-input");

            [Test]
            public void Name_WhenNameIsNotSet_WithFindByNameAttribute_WithMultipleValues() =>
                Go.To<InputPage>()
                    .Find<TextInput<InputPage>>(new FindByNameAttribute("name1", "name2"))
                    .ToSutSubject()
                        .ValueOf(x => x.ComponentName).Should.Equal("FindByName:name1/name2");

            [Test]
            public void Name_WhenNameIsNotSet_WithFindByXPathAttribute_WithMultipleValues() =>
                Go.To<InputPage>()
                    .Find<TextInput<InputPage>>(new FindByXPathAttribute("//a", "//b"))
                    .ToSutSubject()
                        .ValueOf(x => x.ComponentName).Should.Equal("FindByXPath://a or //b");

            [Test]
            public void Name_WhenNameIsNotSet_WithFindFirstAttribute() =>
                Go.To<InputPage>()
                    .Find<H1<InputPage>>()
                    .ToSutSubject()
                        .ValueOf(x => x.ComponentName).Should.Equal("1st");

            [Test]
            public void Name_WhenNameIsNotSet_WithFindLastAttribute() =>
                Go.To<InputPage>()
                    .Find<H1<InputPage>>(new FindLastAttribute())
                    .ToSutSubject()
                        .ValueOf(x => x.ComponentName).Should.Equal("Last");

            [Test]
            public void Name_WhenNameIsNotSet_WithFindByIndexAttribute() =>
               Go.To<InputPage>()
                   .Find<H1<InputPage>>(new FindByIndexAttribute(4))
                   .ToSutSubject()
                       .ValueOf(x => x.ComponentName).Should.Equal("5th");

            [Test]
            public void Name_WhenNameIsNotSet_WithFindByScriptAttribute() =>
               Go.To<InputPage>()
                   .Find<H1<InputPage>>(new FindByScriptAttribute("return true;"))
                   .ToSutSubject()
                       .ValueOf(x => x.ComponentName).Should.Equal("FindByScript");

            [Test]
            public void Name_WhenNameIsSet_WithoutAttributes() =>
                Go.To<InputPage>()
                    .Find<TextInput<InputPage>>(TestName)
                    .ToSutSubject()
                        .ValueOf(x => x.ComponentName).Should.Equal(TestName);

            [Test]
            public void Name_WhenNameIsSet_WithAttributes() =>
                Go.To<InputPage>()
                    .Find<TextInput<InputPage>>(
                        TestName,
                        new ControlDefinitionAttribute("input[@type='email']"))
                    .ToSutSubject()
                        .ValueOf(x => x.ComponentName).Should.Equal(TestName);
        }

        public class FindAll : UITestFixture
        {
            private const string TestName = "some list";

            [Test]
            public void WithoutAttributes() =>
                Go.To<InputPage>()
                    .FindAll<TextInput<InputPage>>()
                        .Count.Should.BeGreater(1);

            [Test]
            public void WithAttributes() =>
                Go.To<InputPage>()
                    .FindAll<TextInput<InputPage>>(new ControlDefinitionAttribute("input[@type='email']"))
                        .Count.Should.Equal(1);

            [Test]
            public void Name_WhenNameIsNotSet_WithoutAttributes()
            {
                var sut = Go.To<InputPage>()
                    .FindAll<TextInput<InputPage>>();

                AssertName(sut, "text input items");
            }

            [Test]
            public void Name_WhenNameIsNotSet_WithAttributes()
            {
                var sut = Go.To<InputPage>()
                    .FindAll<TextInput<InputPage>>(
                        new ControlDefinitionAttribute("input[@type='email']")
                        {
                            ComponentTypeName = "email input"
                        });

                AssertName(sut, "text input items");
            }

            [Test]
            public void Name_WhenNameIsSet_WithoutAttributes()
            {
                var sut = Go.To<InputPage>()
                    .FindAll<TextInput<InputPage>>(TestName);

                AssertName(sut, TestName);
            }

            [Test]
            public void Name_WhenNameIsSet_WithAttributes()
            {
                var sut = Go.To<InputPage>()
                    .FindAll<TextInput<InputPage>>(
                        TestName,
                        new ControlDefinitionAttribute("input[@type='email']"));

                AssertName(sut, TestName);
            }

            private static void AssertName(ControlList<TextInput<InputPage>, InputPage> list, string expected) =>
                list.ToSutSubject()
                    .ValueOf(x => x.Metadata.Name).Should.Equal(expected)
                    .ValueOf(x => x.ComponentPartName).Should.Equal(expected);
        }
    }
}
