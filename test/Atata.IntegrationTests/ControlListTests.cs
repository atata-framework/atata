using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.IntegrationTests
{
    public class ControlListTests : UITestFixture
    {
        [Test]
        public void ControlList_TextInput()
        {
            Go.To<BasicControlsPage>().
                AllTextInputs[1].SetRandom(out string lastName).
                AllTextInputs[1].Should.Equal(lastName).
                Do(x => x.AllTextInputs[0], x => VerifyDoesNotEqual(x, lastName)).
                Do(x => x.AllTextInputs[2], x => VerifyDoesNotEqual(x, lastName)).
                ByLabel.LastName.Should.Equal(lastName);
        }

        [Test]
        public void ControlList_CheckBox()
        {
            int actualCount = 12;

            Go.To<CheckBoxListPage>().
                AllItems.Should.HaveCount(actualCount).
                AllItems.Should.Not.HaveCount(1).
                AllItems.Count.Should.Equal(actualCount).
                AllItems.Count.Should.BeGreaterOrEqual(actualCount).
                AllItems.Count.Should.Not.BeLess(actualCount).
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

        [Test]
        public void ControlList_OfDescendantsAsControls()
        {
            var control = Go.To<ListPage>().ItemsControlOfDescendantsAsControls;

            control.Items.Count.Should.Equal(6);

            var tagNames = control.Items.Select(x => x.Scope.TagName).ToArray();

            tagNames.Where(x => x == "li").Should().HaveCount(3);
            tagNames.Where(x => x == "span").Should().HaveCount(3);
        }

        [Test]
        public void ControlList_OfChildrenAsControls()
        {
            var control = Go.To<ListPage>().ItemsControlOfChildrenAsControls;

            control.Items.Count.Should.Equal(3);

            foreach (var item in control.Items)
                item.Scope.TagName.Should().Be("li");
        }

        [Test]
        public void ControlList_SelectContentsByExtraXPath()
        {
            var component = Go.To<TablePage>().CarsTable.Rows;

            component.SelectContentsByExtraXPath("td[4]").Should.Contain("Saloon");
            component.SelectContentsByExtraXPath("/td[4]", "Categories").Should.HaveCount(18);
            component.SelectContentsByExtraXPath<int>("/td[3]", "Years").Should.Contain(2001, 2007);
        }

        [Test]
        public void ControlList_SelectData_JSPath()
        {
            var component = Go.To<TablePage>().CarsTable.Rows;

            component.SelectData<string>("querySelector('td').getAttribute('data-id')").Should.Contain("2");
            component.SelectData<int?>("querySelector('td').getAttribute('data-id')").Should.Contain(1, 2, null);
        }

        [Test]
        public void ControlList_SelectDataByExtraXPath()
        {
            var component = Go.To<TablePage>().CarsTable.Rows;

            component.SelectDataByExtraXPath<string>("td[1]", "getAttribute('data-id')").Should.Contain("2");
            component.SelectDataByExtraXPath<int?>("/td[1]", "getAttribute('data-id')").Should.Contain(1, 2, null);
            component.SelectDataByExtraXPath<int?>("/td[1][@data-id]", "getAttribute('data-id')").Should.EqualSequence(1, 2, 3, 4);
        }

        [Test]
        public void ControlList_GetByXPathCondition()
        {
            var component = Go.To<TablePage>().CarsTable.Rows;

            component.GetByXPathCondition("Nissan", "td[1][text()='Nissan']").CarMake.Should.Equal("Nissan");
        }

        [Test]
        public void ControlList_GetAllByXPathCondition()
        {
            var component = Go.To<TablePage>().CarsTable.Rows;

            component.GetAllByXPathCondition("Nissan", "td[1][text()='Nissan']").Should.HaveCount(2);
        }

        [Test]
        public void ControlList_Empty()
        {
            var component = Go.To<TablePage>().EmptyTable.Rows;

            using (StopwatchAsserter.WithinSeconds(0))
                component.Should.BeEmpty();

            using (StopwatchAsserter.WithinSeconds(0))
                component.Count.Should.Equal(0);

            using (StopwatchAsserter.WithinSeconds(0))
                component.Should.HaveCount(0);

            using (StopwatchAsserter.WithinSeconds(0))
                component.AsEnumerable().Should().BeEmpty();
        }

        [Test]
        public void PredicateIndexer_UsingExternalIndexer()
        {
            var sut = Go.To<TablePage>().NumberedTable.Rows;

            var dictionaries = new[]
            {
                new Dictionary<string, string>
                {
                    ["Name"] = "Item 1",
                    ["Number"] = "1"
                },
                new Dictionary<string, string>
                {
                    ["Name"] = "Item 2",
                    ["Number"] = "2"
                }
            };

            foreach (var dictionary in dictionaries)
            {
                sut[x => x.Name.Value.Contains(dictionary["Name"])]
                    .Name.Should.Equal(dictionary["Name"]);
            }
        }

        public class UsesScopeCache : UITestFixture
        {
            private ControlList<TablePage.NumberedTableRow, TablePage> _sut;

            protected override void OnSetUp()
            {
                var table = Go.To<TablePage>().NumberedTable;
                table.Metadata.Push(new UsesScopeCacheAttribute());

                _sut = table.Rows;
                _sut.Metadata.Push(new UsesScopeCacheAttribute { TargetSelfAndChildren = true });
            }

            [Test]
            public void ReuseItem()
            {
                var item = _sut[x => x.Name == "Item 2"];
                item.Number.Should.Be(2);
                item.Number.Should.Be(2);

                AssertThatLastLogSectionIsVerificationWithExecuteBehavior();
            }

            [Test]
            public void SameItem_BySamePredicate()
            {
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);

                var entries = GetLastLogEntries(10);
                entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
                entries[1].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[2].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[3].SectionEnd.Should().Be(entries[2].SectionStart);
                entries[4].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[5].SectionEnd.Should().Be(entries[4].SectionStart);
                entries[6].SectionStart.Should().BeOfType<ElementFindLogSection>();
                entries[7].SectionEnd.Should().Be(entries[6].SectionStart);
                entries[8].SectionEnd.Should().Be(entries[1].SectionStart);
                entries[9].SectionEnd.Should().Be(entries[0].SectionStart);
            }

            [Test]
            public void SameItem_BySameIndex()
            {
                _sut[1].Number.Should.Be(2);
                _sut[1].Number.Should.Be(2);

                AssertThatLastLogSectionIsVerificationWithExecuteBehavior();
            }

            [Test]
            public void SameItem_BySameXPath()
            {
                _sut.GetByXPathCondition("td[1][.='Item 2']").Number.Should.Be(2);
                _sut.GetByXPathCondition("td[1][.='Item 2']").Number.Should.Be(2);

                AssertThatLastLogSectionIsVerificationWithExecuteBehavior();
            }

            [Test]
            public void SameItem_ByDifferentPredicate()
            {
                _sut[x => x.Number == 2 && x.Name == "Item 2"].Should.BePresent();
                _sut[x => x.Number == 2].Should.BePresent();

                var entries = GetLastLogEntries(6);
                entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
                entries[1].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[2].SectionEnd.Should().Be(entries[1].SectionStart);
                entries[3].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[4].SectionEnd.Should().Be(entries[3].SectionStart);
                entries[5].SectionEnd.Should().Be(entries[0].SectionStart);
            }

            [Test]
            public void PreviousItem_BySimilarPredicate()
            {
                _sut[x => x.Name == "Item 3"].Should.BePresent();
                _sut[x => x.Name == "Item 2"].Should.BePresent();

                var entries = GetLastLogEntries(6);
                entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
                entries[1].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[2].SectionEnd.Should().Be(entries[1].SectionStart);
                entries[3].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[4].SectionEnd.Should().Be(entries[3].SectionStart);
                entries[5].SectionEnd.Should().Be(entries[0].SectionStart);
            }

            [Test]
            public void GetCount_2Times()
            {
                _sut.Count.Should.Be(3);
                _sut.Count.Should.Be(3);

                AssertThatLastLogSectionIsVerificationAndEmpty();
            }

            [Test]
            public void GetCount_AfterGettingItem()
            {
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);
                _sut.Count.Should.Be(3);

                AssertThatLastLogSectionIsVerificationAndEmpty();
            }

            [Test]
            public void AfterClearCache()
            {
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);
                _sut.ClearCache();
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);

                var entries = GetLastLogEntries(3);
                entries[0].SectionEnd.Should().BeOfType<ElementFindLogSection>();
                entries[1].SectionEnd.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[2].SectionEnd.Should().BeOfType<VerificationLogSection>();
            }

            [Test]
            public void AfterClearCache_OfPageObject()
            {
                _sut[1].Number.Should.Be(2);
                _sut.Component.Owner.ClearCache();
                _sut[1].Number.Should.Be(2);

                var entries = GetLastLogEntries(3);
                entries[0].SectionEnd.Should().BeOfType<ElementFindLogSection>();
                entries[1].SectionEnd.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[2].SectionEnd.Should().BeOfType<VerificationLogSection>();
            }
        }

        public class UsesValueCache : UITestFixture
        {
            private ControlList<TablePage.NumberedTableRow, TablePage> _sut;

            protected override void OnSetUp()
            {
                var table = Go.To<TablePage>().NumberedTable;

                _sut = table.Rows;
                _sut.Metadata.Push(new UsesValueCacheAttribute { TargetChildren = true });
            }

            [Test]
            public void ReuseItem()
            {
                var item = _sut[x => x.Name == "Item 2"];
                item.Number.Should.Be(2);
                item.Number.Should.Be(2);

                AssertThatLastLogSectionIsVerificationAndEmpty();
            }

            [Test]
            public void SameItem_BySamePredicate()
            {
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);

                AssertThatLastLogSectionIsVerificationWithExecuteBehaviorAnd3ElementFindSections();
            }

            [Test]
            public void SameItem_BySameIndex()
            {
                _sut[1].Number.Should.Be(2);
                _sut[1].Number.Should.Be(2);

                AssertThatLastLogSectionIsVerificationWithExecuteBehaviorAnd3ElementFindSections();
            }

            [Test]
            public void SameItem_BySameXPath()
            {
                _sut.GetByXPathCondition("td[1][.='Item 2']").Number.Should.Be(2);
                _sut.GetByXPathCondition("td[1][.='Item 2']").Number.Should.Be(2);

                AssertThatLastLogSectionIsVerificationWithExecuteBehaviorAnd3ElementFindSections();
            }

            [Test]
            public void SameItem_ByDifferentPredicate()
            {
                _sut[x => x.Number == 2 && x.Name == "Item 2"].Should.BePresent();
                _sut[x => x.Number == 2].Should.BePresent();

                AssertThatLastLogSectionIsVerificationWith2ElementFindSections();
            }

            [Test]
            public void PreviousItem_BySimilarPredicate()
            {
                _sut[x => x.Name == "Item 3"].Should.BePresent();
                _sut[x => x.Name == "Item 2"].Should.BePresent();

                AssertThatLastLogSectionIsVerificationWith2ElementFindSections();
            }

            [Test]
            public void GetCount_2Times()
            {
                _sut.Count.Should.Be(3);
                _sut.Count.Should.Be(3);

                AssertThatLastLogSectionIsVerificationWith2ElementFindSections();
            }

            [Test]
            public void GetCount_AfterGettingItem()
            {
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);
                _sut.Count.Should.Be(3);

                AssertThatLastLogSectionIsVerificationWith2ElementFindSections();
            }

            [Test]
            public void AfterClearCache()
            {
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);
                _sut.ClearCache();
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);

                var entries = GetLastLogEntries(3);
                entries[0].SectionEnd.Should().BeOfType<ElementFindLogSection>();
                entries[1].SectionEnd.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[2].SectionEnd.Should().BeOfType<VerificationLogSection>();
            }

            [Test]
            public void AfterClearCache_OfPageObject()
            {
                _sut[1].Number.Should.Be(2);
                _sut.Component.Owner.ClearCache();
                _sut[1].Number.Should.Be(2);

                var entries = GetLastLogEntries(3);
                entries[0].SectionEnd.Should().BeOfType<ElementFindLogSection>();
                entries[1].SectionEnd.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[2].SectionEnd.Should().BeOfType<VerificationLogSection>();
            }
        }

        public class UsesCache : UITestFixture
        {
            private ControlList<TablePage.NumberedTableRow, TablePage> _sut;

            protected override void OnSetUp()
            {
                var table = Go.To<TablePage>().NumberedTable;
                table.Metadata.Push(new UsesCacheAttribute());

                _sut = table.Rows;
                _sut.Metadata.Push(new UsesCacheAttribute { TargetSelfAndChildren = true });
            }

            [Test]
            public void ReuseItem()
            {
                var item = _sut[x => x.Name == "Item 2"];
                item.Number.Should.Be(2);
                item.Number.Should.Be(2);

                AssertThatLastLogSectionIsVerificationAndEmpty();
            }

            [Test]
            public void SameItem_BySamePredicate()
            {
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);

                var entries = GetLastLogEntries(6);
                entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
                entries[1].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[2].SectionStart.Should().BeOfType<ElementFindLogSection>();
                entries[3].SectionEnd.Should().Be(entries[2].SectionStart);
                entries[4].SectionEnd.Should().Be(entries[1].SectionStart);
                entries[5].SectionEnd.Should().Be(entries[0].SectionStart);
            }

            [Test]
            public void SameItem_BySameIndex()
            {
                _sut[1].Number.Should.Be(2);
                _sut[1].Number.Should.Be(2);

                AssertThatLastLogSectionIsVerificationAndEmpty();
            }

            [Test]
            public void SameItem_BySameXPath()
            {
                _sut.GetByXPathCondition("td[1][.='Item 2']").Number.Should.Be(2);
                _sut.GetByXPathCondition("td[1][.='Item 2']").Number.Should.Be(2);

                AssertThatLastLogSectionIsVerificationAndEmpty();
            }

            [Test]
            public void SameItem_ByDifferentPredicate()
            {
                _sut[x => x.Number == 2 && x.Name == "Item 2"].Should.BePresent();
                _sut[x => x.Number == 2].Should.BePresent();

                AssertThatLastLogSectionIsVerificationAndEmpty();
            }

            [Test]
            public void PreviousItem_BySimilarPredicate()
            {
                _sut[x => x.Name == "Item 3"].Should.BePresent();
                _sut[x => x.Name == "Item 2"].Should.BePresent();

                AssertThatLastLogSectionIsVerificationAndEmpty();
            }

            [Test]
            public void GetCount_2Times()
            {
                _sut.Count.Should.Be(3);
                _sut.Count.Should.Be(3);

                AssertThatLastLogSectionIsVerificationAndEmpty();
            }

            [Test]
            public void GetCount_AfterGettingItem()
            {
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);
                _sut.Count.Should.Be(3);

                AssertThatLastLogSectionIsVerificationAndEmpty();
            }

            [Test]
            public void AfterClearCache()
            {
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);
                _sut.ClearCache();
                _sut[x => x.Name == "Item 2"].Number.Should.Be(2);

                var entries = GetLastLogEntries(3);
                entries[0].SectionEnd.Should().BeOfType<ElementFindLogSection>();
                entries[1].SectionEnd.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[2].SectionEnd.Should().BeOfType<VerificationLogSection>();
            }

            [Test]
            public void AfterClearCache_OfPageObject()
            {
                _sut[1].Number.Should.Be(2);
                _sut.Component.Owner.ClearCache();
                _sut[1].Number.Should.Be(2);

                var entries = GetLastLogEntries(3);
                entries[0].SectionEnd.Should().BeOfType<ElementFindLogSection>();
                entries[1].SectionEnd.Should().BeOfType<ExecuteBehaviorLogSection>();
                entries[2].SectionEnd.Should().BeOfType<VerificationLogSection>();
            }
        }
    }
}
