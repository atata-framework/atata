using NUnit.Framework;

namespace Atata.Tests
{
    public class ControlListTest : AutoTest
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
    }
}
