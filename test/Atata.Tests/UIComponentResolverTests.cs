using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class UIComponentResolverTests
    {
        public static IEnumerable<TestCaseData> GetResolveControlNameData()
        {
            List<TestCaseData> items = new List<TestCaseData>();

            void Add(Expression<Func<TestPage.TestControl, bool>> expression, string expectedResult)
            {
                items.Add(new TestCaseData(expression).Returns(expectedResult));
            }

            string itemName = "item";
            TestModel item = new TestModel { Name = "item" };
            string[] itemArray = { "item" };

            Add(x => x.Item == "item", "Item == \"item\"");
            Add(x => x.Item == "item 1" || x.Item == "item 2", "Item == \"item 1\" || Item == \"item 2\"");
            Add(x => x.Item == itemName, "Item == \"item\"");
            Add(x => x.Item == item.Name, "Item == \"item\"");
            Add(x => x.Item.Attributes["data-id"] == "15", "Item.Attributes[\"data-id\"] == \"15\"");
            Add(x => x.Item.Attributes["data-id"] == itemArray[0], "Item.Attributes[\"data-id\"] == itemArray[0]");
            Add(x => x.Item.Attributes.Checked, "Item.Attributes.Checked");
            Add(x => x.Item.Attributes.Checked == true, "Item.Attributes.Checked == true");
            Add(x => x.Item.Attributes.GetValue<DateTime>("data-date") <= DateTime.Today, "Item.Attributes.GetValue(\"data-date\") <= DateTime.Today");
            Add(x => x.Item.Value.Length == StaticClass.GetSomething(), $"Item.Value.Length == {nameof(UIComponentResolverTests)}.{nameof(StaticClass)}.{nameof(StaticClass.GetSomething)}()");
            Add(x => x.Item.Value.Contains(StaticClass.GetSomething(item.Name)), $"Item.Value.Contains({nameof(UIComponentResolverTests)}.{nameof(StaticClass)}.{nameof(StaticClass.GetSomething)}(\"item\"))");

            return items;
        }

        [TestCaseSource(nameof(GetResolveControlNameData))]
        public string UIComponentResolver_ResolveControlName(Expression<Func<TestPage.TestControl, bool>> expression)
        {
            return UIComponentResolver.ResolveControlName<TestPage.TestControl, TestPage>(expression);
        }

        public static class StaticClass
        {
            public static int GetSomething()
            {
                return 4;
            }

            public static string GetSomething(string value)
            {
                return value;
            }
        }

        public class TestPage : Page<TestPage>
        {
            public TestControl Child { get; private set; }

            public class TestControl : Control<TestPage>
            {
                public Text<TestPage> Item { get; private set; }
            }
        }

        public class TestModel
        {
            public string Name { get; set; }
        }
    }
}
