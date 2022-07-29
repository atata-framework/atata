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
            List<TestCaseData> items = new();

            TestCaseData TestPredicate(Expression<Func<TestComponent, object>> expression)
            {
                TestCaseData data = new(expression);
                items.Add(data);
                return data;
            }

            string itemName = "item";
            TestModel item = new() { Name = "item" };
            string[] itemArray = { "item" };

            TestPredicate(x => x.Item1 == "item")
                .Returns("Item1 == \"item\"");
            TestPredicate(x => x.Item1 == "item 1" || x.Item1 == "item 2")
                .Returns("Item1 == \"item 1\" || Item1 == \"item 2\"");
            TestPredicate(x => x.Item1 == itemName)
                .Returns("Item1 == \"item\"");
            TestPredicate(x => x.Item1 == item.Name)
                .Returns("Item1 == item.Name");
            TestPredicate(x => x["data-id"])
                .Returns("[\"data-id\"]");
            TestPredicate(x => x["data-id"] == null)
                .Returns("[\"data-id\"] == null");
            TestPredicate(x => x.Item1.Attributes["data-id"] == "15")
                .Returns("Item1.Attributes[\"data-id\"] == \"15\"");
            TestPredicate(x => x.Item1.Attributes["data-id"] == itemArray[0])
                .Returns("Item1.Attributes[\"data-id\"] == itemArray[0]");
            TestPredicate(x => x.Item1.Attributes.Checked)
                .Returns("Item1.Attributes.Checked");
            TestPredicate(x => x.Item1.Attributes.Checked == true)
                .Returns("Item1.Attributes.Checked == true");
            TestPredicate(x => x.Item1.Attributes.GetValue<DateTime>("data-date") <= DateTime.Today)
                .Returns("Item1.Attributes.GetValue(\"data-date\") <= DateTime.Today");
            TestPredicate(x => x.Item1.Value.Length == StaticClass.GetSomething())
                .Returns($"Item1.Value.Length == {nameof(ObjectExpressionStringBuilderTests)}.{nameof(StaticClass)}.{nameof(StaticClass.GetSomething)}()");
            TestPredicate(x => x.Item1.Value.Contains(StaticClass.GetSomething(item.Name)))
                .Returns($"Item1.Value.Contains({nameof(ObjectExpressionStringBuilderTests)}.{nameof(StaticClass)}.{nameof(StaticClass.GetSomething)}(item.Name))");
            TestPredicate(x => x.Item1.Value.Any(ch => ch == '!'))
                .Returns("Item1.Value.Any(ch => ch == '!')");
            TestPredicate(x => x.IsIt())
                .Returns("IsIt()");
            TestPredicate(x => x.Item1.GetContent() == null)
                .Returns("Item1.GetContent() == null");
            TestPredicate(x => StaticClass.IsIt(x.Item1))
                .Returns($"{nameof(ObjectExpressionStringBuilderTests)}.{nameof(StaticClass)}.IsIt(Item1)");
            TestPredicate(x => StaticClass.IsIt(x))
                .Returns($"{nameof(ObjectExpressionStringBuilderTests)}.{nameof(StaticClass)}.IsIt(x)");
            TestPredicate(x => x.ToString(TermCase.Kebab) != null)
                .Returns("ToString(TermCase.Kebab) != null");
            TestPredicate(x => x.Item1.ToString(TermCase.Kebab) != null)
                .Returns("Item1.ToString(TermCase.Kebab) != null");

            return items;
        }

        [TestCaseSource(nameof(GetExpressionTestCases))]
        public string ExpressionToString(Expression<Func<TestComponent, object>> expression)
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
            public TestItem Item1 { get; private set; }

            public abstract object this[string index] { get; }

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
