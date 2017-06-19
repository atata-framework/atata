using NUnit.Framework;

namespace Atata.Tests
{
    public class ControlListTests : UITestFixture
    {
        [Test]
        public void ControlList_TextInput()
        {
            string lastName;

            Go.To<BasicControlsPage>().
                AllTextInputs[1].SetRandom(out lastName).
                AllTextInputs[1].Should.Equal(lastName).
                Do(x => x.AllTextInputs[0], x => VerifyDoesNotEqual(x, lastName)).
                Do(x => x.AllTextInputs[2], x => VerifyDoesNotEqual(x, lastName)).
                ByLabel.LastName.Should.Equal(lastName);
        }

        [Test]
        public void ControlList_CheckBox()
        {
            Go.To<CheckBoxListPage>().
                AllItems.Should.HaveCount(6).
                AllItems.Should.Not.HaveCount(1).
                AllItems.Count.Should.Equal(6).
                AllItems.Count.Should.BeGreaterOrEqual(6).
                AllItems.Count.Should.Not.BeLess(6).
                AllItems.Should.Not.Contain(true).
                AllItems.Should.Not.Contain(x => x.IsChecked).
                AllItems[x => x.IsChecked].Should.Not.Exist().
                AllItems[2].Check().
                AllItems.Should.Contain(x => x.IsChecked).
                AllItems[2].IsChecked.Should.BeTrue();
        }

        [Test]
        public void ControlList_RadioButton()
        {
            Go.To<RadioButtonListPage>().
                IntegerItemsControl.Should.Exist().
                IntegerItemsControl.Items.Should.HaveCount(4).
                IntegerItemsControl.Items.Should.Not.HaveCount(1).
                IntegerItemsControl.Items.Count.Should.Equal(4).
                IntegerItemsControl.Items.Should.Not.Contain(true).
                IntegerItemsControl.Items.Should.Not.Contain(x => x.IsChecked).
                IntegerItemsControl.Items[x => x.IsChecked].Should.Not.Exist().
                IntegerItemsControl.Items[3].Check().
                IntegerItemsControl.Items.Should.Contain(x => x.IsChecked).
                IntegerItemsControl.Items[3].IsChecked.Should.BeTrue().
                IntegerItemsControl.Items[1].Check().
                IntegerItemsControl.Items[3].IsChecked.Should.BeFalse().
                IntegerItemsControl.Items[1].Should.BeChecked();
        }

        [Test]
        public void ControlList_WithDeclaredAttributes()
        {
            Go.To<ListPage>().
                ProductNameTextContolList[0].Should.Equal("Phone").
                ProductNameTextContolList[1].Should.Equal("Book").
                ProductNameTextContolList.Count.Should.Equal(3).
                ProductNameTextContolList.Should.EqualSequence("Phone", "Book", "Table").
                ProductPercentNumberContolList.Should.EqualSequence(0.05m, 0.10m, 0.15m).
                ProductPercentNumberContolList[1].Should.Equal(0.10m);
        }
    }
}
