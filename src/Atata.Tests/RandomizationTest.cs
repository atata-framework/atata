using NUnit.Framework;

namespace Atata.Tests
{
    public class RandomizationTest : AutoTest
    {
        private const int MaxTriesNumber = 100;

        private RandomizationPage page;

        protected override void OnSetUp()
        {
            page = Go.To<RandomizationPage>();
        }

        [Test]
        public void Randomization_Enum_Single()
        {
            var control = page.SimpleEnum;

            RandomizationPage.CheckBoxOptions value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                RandomizationPage.CheckBoxOptions newValue;

                control.SetRandom(out newValue);
                control.Should.Equal(newValue);
                page.CheckedItemsCount.Should.BeInRange(0, 1);

                if (newValue != value)
                    return;
            }

            Assert.Fail();
        }

        [Test]
        public void Randomization_Enum_Multiple()
        {
            var control = page.MultipleEnums;

            RandomizationPage.CheckBoxOptions value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                RandomizationPage.CheckBoxOptions newValue;

                control.SetRandom(out newValue);
                control.Should.Equal(newValue);
                page.CheckedItemsCount.Should.BeInRange(2, 4);

                if (newValue != value)
                    return;
            }

            Assert.Fail();
        }

        [Test]
        public void Randomization_Enum_MultipleWithExcluding()
        {
            var control = page.MultipleEnumsExcludingNoneBDF;

            RandomizationPage.CheckBoxOptions value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                RandomizationPage.CheckBoxOptions newValue;

                control.SetRandom(out newValue);
                control.Should.Equal(newValue);
                page.CheckedItemsCount.Should.Equal(2);
                control.Should.Not.HaveChecked(
                    RandomizationPage.CheckBoxOptions.None |
                    RandomizationPage.CheckBoxOptions.OptionB |
                    RandomizationPage.CheckBoxOptions.OptionD |
                    RandomizationPage.CheckBoxOptions.OptionF);

                if (newValue != value)
                    return;
            }

            Assert.Fail();
        }

        [Test]
        public void Randomization_Enum_MultipleWithIncluding()
        {
            var control = page.MultipleEnumsIncludingABDEF;

            RandomizationPage.CheckBoxOptions value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                RandomizationPage.CheckBoxOptions newValue;

                control.SetRandom(out newValue);
                control.Should.Equal(newValue);
                page.CheckedItemsCount.Should.Equal(3);
                control.Should.Not.HaveChecked(
                    RandomizationPage.CheckBoxOptions.None |
                    RandomizationPage.CheckBoxOptions.OptionC);

                if (newValue != value)
                    return;
            }

            Assert.Fail();
        }

        [Test]
        public void Randomization_Enum_Nullable()
        {
            var control = page.EnumSelect;

            RandomizationPage.SelectOption value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                RandomizationPage.SelectOption newValue;

                control.SetRandom(out newValue);
                control.Should.Equal(newValue);

                if (newValue != value)
                    return;
            }

            Assert.Fail();
        }

        [Test]
        public void Randomization_String_WithIncluding()
        {
            var control = page.TextSelect;

            string value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                string newValue;

                control.SetRandom(out newValue);
                control.Should.Equal(newValue);
                control.Should.Not.Equal("Option D");

                if (newValue != value)
                    return;
            }

            Assert.Fail();
        }

        [Test]
        public void Randomization_Int_WithIncluding()
        {
            var control = page.IntSelect;

            int value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                int newValue;

                control.SetRandom(out newValue);
                control.Should.Equal(newValue);
                control.Should.BeLessOrEqual(3);

                if (newValue != value)
                    return;
            }

            Assert.Fail();
        }
    }
}
