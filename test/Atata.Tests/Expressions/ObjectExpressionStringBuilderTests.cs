using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

namespace Atata.Tests.Expressions
{
    public class ObjectExpressionStringBuilderTests
    {
        public static IEnumerable<TestCaseData> GetExpressionTestCases()
        {
            List<TestCaseData> items = new List<TestCaseData>();

            TestCaseData TestPredicate(Expression<Func<TestComponent, bool>> expression)
            {
                TestCaseData data = new TestCaseData(expression);
                items.Add(data);
                return data;
            }

            string itemName = "item";
            TestModel item = new TestModel { Name = "item" };
            string[] itemArray = { "item" };

            TestPredicate(x => x.Item == "item")
                .Returns("Item == \"item\"");
            TestPredicate(x => x.Item == "item 1" || x.Item == "item 2")
                .Returns("Item == \"item 1\" || Item == \"item 2\"");
            TestPredicate(x => x.Item == itemName)
                .Returns("Item == \"item\"");
            TestPredicate(x => x.Item == item.Name)
                .Returns("Item == item.Name");
            TestPredicate(x => x.Item.Attributes["data-id"] == "15")
                .Returns("Item.Attributes[\"data-id\"] == \"15\"");
            TestPredicate(x => x.Item.Attributes["data-id"] == itemArray[0])
                .Returns("Item.Attributes[\"data-id\"] == itemArray[0]");
            TestPredicate(x => x.Item.Attributes.Checked)
                .Returns("Item.Attributes.Checked");
            TestPredicate(x => x.Item.Attributes.Checked == true)
                .Returns("Item.Attributes.Checked == true");
            TestPredicate(x => x.Item.Attributes.GetValue<DateTime>("data-date") <= DateTime.Today)
                .Returns("Item.Attributes.GetValue(\"data-date\") <= DateTime.Today");
            TestPredicate(x => x.Item.Value.Length == StaticClass.GetSomething())
                .Returns($"Item.Value.Length == {nameof(ObjectExpressionStringBuilderTests)}.{nameof(StaticClass)}.{nameof(StaticClass.GetSomething)}()");
            TestPredicate(x => x.Item.Value.Contains(StaticClass.GetSomething(item.Name)))
                .Returns($"Item.Value.Contains({nameof(ObjectExpressionStringBuilderTests)}.{nameof(StaticClass)}.{nameof(StaticClass.GetSomething)}(item.Name))");
            TestPredicate(x => x.Item.Value.Any(ch => ch == '!'))
                .Returns("Item.Value.Any(ch => ch == '!')");
            TestPredicate(x => x.IsIt())
                .Returns("IsIt()");
            TestPredicate(x => x.Item.GetContent() == null)
                .Returns("Item.GetContent() == null");
            TestPredicate(x => StaticClass.IsIt(x.Item))
                .Returns($"{nameof(ObjectExpressionStringBuilderTests)}.{nameof(StaticClass)}.IsIt(Item)");
            TestPredicate(x => StaticClass.IsIt(x))
                .Returns($"{nameof(ObjectExpressionStringBuilderTests)}.{nameof(StaticClass)}.IsIt(x)");
            TestPredicate(x => x.ToString(TermCase.Kebab) != null)
                .Returns("ToString(TermCase.Kebab) != null");
            TestPredicate(x => x.Item.ToString(TermCase.Kebab) != null)
                .Returns("Item.ToString(TermCase.Kebab) != null");

            return items;
        }

        [TestCaseSource(nameof(GetExpressionTestCases))]
        public string ExpressionToString(Expression<Func<TestComponent, bool>> expression)
        {
            return ObjectExpressionStringBuilder.ExpressionToString(expression);
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

            public static bool IsIt(object value)
            {
                return value is string;
            }
        }

        public abstract class TestComponent
        {
            public TestItem Item { get; private set; }

            public abstract bool IsIt();
        }

        public class TestItem
        {
            public TestItemAttributes Attributes { get; private set; }

            public string Value { get; private set; }

            public static implicit operator string(TestItem item) =>
                item.ToString();

            public string GetContent() =>
                ToString();
        }

        public abstract class TestItemAttributes
        {
            public bool Checked { get; private set; }

            public abstract string this[string index] { get; }

            public abstract TValue GetValue<TValue>(string attributeName);
        }

        public class TestModel
        {
            public string Name { get; set; }
        }
    }
}
