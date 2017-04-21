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

            RandomizationPage.Options value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                RandomizationPage.Options newValue;

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

            RandomizationPage.Options value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                RandomizationPage.Options newValue;

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

            RandomizationPage.Options value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                RandomizationPage.Options newValue;

                control.SetRandom(out newValue);
                control.Should.Equal(newValue);
                page.CheckedItemsCount.Should.Equal(2);
                control.Should.Not.HaveChecked(
                    RandomizationPage.Options.None |
                    RandomizationPage.Options.OptionB |
                    RandomizationPage.Options.OptionD |
                    RandomizationPage.Options.OptionF);

                if (newValue != value)
                    return;
            }

            Assert.Fail();
        }

        [Test]
        public void Randomization_Enum_MultipleWithIncluding()
        {
            var control = page.MultipleEnumsIncludingABDEF;

            RandomizationPage.Options value;
            control.SetRandom(out value);

            for (int i = 0; i < MaxTriesNumber; i++)
            {
                RandomizationPage.Options newValue;

                control.SetRandom(out newValue);
                control.Should.Equal(newValue);
                page.CheckedItemsCount.Should.Equal(3);
                control.Should.Not.HaveChecked(
                    RandomizationPage.Options.None |
                    RandomizationPage.Options.OptionC);

                if (newValue != value)
                    return;
            }

            Assert.Fail();
        }
    }
}
