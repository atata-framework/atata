﻿namespace Atata.UnitTests.Expressions;

public static class ImprovedExpressionStringBuilderTests
{
    private static readonly string s_testFieldValue = "FldStr";

    [Flags]
    public enum TestFlagValues
    {
        None,
        A = 1,
        B = 2,
        C = 4,
        BC = 6
    }

    private static List<TestCaseData> GetExpressionTestCases()
    {
        List<TestCaseData> items = [];

        TestCaseData Test(Expression expression)
        {
            TestCaseData data = new(expression);
            items.Add(data);
            return data;
        }

        TestCaseData TestPredicate(Expression<Func<TestComponent, bool>> expression) =>
            Test(expression);

        TestCaseData TestModelWithIndexPredicate(Expression<Func<TestModel, int, bool>> expression) =>
            Test(expression);
        TestCaseData TestModelSelector(Expression<Func<TestModel, object>> expression) =>
            Test(expression);

        // Comparison:
        TestPredicate(x => x.Item == "item")
            .Returns("x => x.Item == \"item\"");
        TestPredicate(x => x.Item == "item 0" || x.Item == "item 1")
            .Returns("x => x.Item == \"item 0\" || x.Item == \"item 1\"");
        TestPredicate(x => x.Item == "item 1" && x.Item2 == "item 2")
            .Returns("x => x.Item == \"item 1\" && x.Item2 == \"item 2\"");
        TestPredicate(x => (x.Item == "item 0" || x.Item == "item 1") && x.Item2 == "item 2")
            .Returns("x => ((x.Item == \"item 0\") || (x.Item == \"item 1\")) && (x.Item2 == \"item 2\")");
        TestPredicate(x => x.Item.Attributes.Checked)
            .Returns("x => x.Item.Attributes.Checked");
        TestPredicate(x => !x.Item.Attributes.Checked)
            .Returns("x => !x.Item.Attributes.Checked");
        TestPredicate(x => x.Item.Attributes.Checked == true)
            .Returns("x => x.Item.Attributes.Checked == true");

        // Variable:
        string itemName = "VarStr";
        TestModel item = new() { Name = "PropStr" };
        ValueProvider<string, object>? valueItem = null;
        bool? nullableBool = null;
        bool? nullableBoolIsTrue = true;

        TestPredicate(x => x.Item == itemName)
            .Returns("x => x.Item == \"VarStr\"");
        TestPredicate(x => x.Item == s_testFieldValue)
            .Returns("x => x.Item == \"FldStr\"");
        TestPredicate(x => x.Item != item.Name)
            .Returns("x => x.Item != item.Name");
        TestPredicate(x => x.Item == valueItem!)
            .Returns("x => x.Item == valueItem");
        TestPredicate(x => x.Item == valueItem!.Value)
            .Returns("x => x.Item == valueItem.Value");
        TestPredicate(x => x.Item.Attributes.Checked == nullableBool)
            .Returns("x => x.Item.Attributes.Checked == null");
        TestPredicate(x => x.Item.Attributes.Checked == nullableBoolIsTrue)
            .Returns("x => x.Item.Attributes.Checked == true");

        // Indexer:
        string[] itemArray = ["item"];

        TestPredicate(x => x.Item.Attributes["data-id"] == "15")
            .Returns("x => x.Item.Attributes[\"data-id\"] == \"15\"");
        TestPredicate(x => x.Item.Attributes["data-id"] == itemArray[0])
            .Returns("x => x.Item.Attributes[\"data-id\"] == itemArray[0]");
        TestPredicate(x => x.Item.Value[1] == 'a')
            .Returns("x => x.Item.Value[1] == 'a'");

        // Instance method:
        TestPredicate(x => x.Item.Attributes.GetValue<DateTime>("data-date") <= DateTime.Today)
            .Returns("x => x.Item.Attributes.GetValue(\"data-date\") <= DateTime.Today");

        // Instance method with out parameter:
        int outResult;

        TestPredicate(x => x.TryGetValue("key", out outResult))
            .Returns("x => x.TryGetValue(\"key\", out outResult)");

        // Instance method with out parameter:
        int refValue = 1;

        TestPredicate(x => x.UseRefValue("key", ref refValue))
            .Returns("x => x.UseRefValue(\"key\", ref refValue)");

        // Static method:
        TestPredicate(x => x.Item.Value.Length == StaticClass.GetInt())
            .Returns($"x => x.Item.Value.Length == {nameof(ImprovedExpressionStringBuilderTests)}.{nameof(StaticClass)}.{nameof(StaticClass.GetInt)}()");
        TestPredicate(x => x.Item.Value.Contains(StaticClass.GetString(item.Name)))
            .Returns($"x => x.Item.Value.Contains({nameof(ImprovedExpressionStringBuilderTests)}.{nameof(StaticClass)}.{nameof(StaticClass.GetString)}(item.Name))");
        TestPredicate(x => StaticClass.GetBool())
            .Returns($"x => {nameof(ImprovedExpressionStringBuilderTests)}.{nameof(StaticClass)}.{nameof(StaticClass.GetBool)}()");

        // Enum comparison:
        TestPredicate(x => x.Flags == TestFlagValues.B)
            .Returns("x => x.Flags == TestFlagValues.B");
        TestPredicate(x => x.Flags == (TestFlagValues.A | TestFlagValues.B))
            .Returns("x => x.Flags == (TestFlagValues.A | TestFlagValues.B)");
        TestPredicate(x => x.Flags == (TestFlagValues.B | TestFlagValues.C))
            .Returns("x => x.Flags == TestFlagValues.BC");

        // Enum as argument:
        TestPredicate(x => x.IsIt(TestFlagValues.B))
            .Returns("x => x.IsIt(TestFlagValues.B)");
        TestPredicate(x => x.IsIt(TestFlagValues.A | TestFlagValues.B))
            .Returns("x => x.IsIt(TestFlagValues.A | TestFlagValues.B)");
        TestPredicate(x => x.IsIt(TestFlagValues.B | TestFlagValues.C))
            .Returns("x => x.IsIt(TestFlagValues.BC)");
#pragma warning disable CA1860
        TestPredicate(x => x.Item.Value.ToString(TermCase.Upper).Any())
            .Returns("x => x.Item.Value.ToString(TermCase.Upper).Any()");
#pragma warning restore CA1860

        // Char:
        char charValue = '!';

        TestPredicate(x => x.Item.Value.Any(ch => ch == '!'))
            .Returns("x => x.Item.Value.Any(ch => ch == '!')");
        TestPredicate(x => x.Item.Value.Any(ch => charValue <= ch))
            .Returns("x => x.Item.Value.Any(ch => '!' <= ch)");
        TestPredicate(x => x.Item.Value.Contains('!'))
            .Returns("x => x.Item.Value.Contains('!')");

        // Two arguments:
        TestModelWithIndexPredicate((_, i) => i % 2 == 0)
            .Returns("(_, i) => (i % 2) == 0");
        TestModelWithIndexPredicate((x, i) => i >= 0 && Equals(x, null))
            .Returns("(x, i) => i >= 0 && Equals(x, null)");

        // Object construction:
        TestModelSelector(x => new TestModel())
            .Returns("x => new TestModel()");
        TestModelSelector(x => new TestModel(x.Name))
            .Returns("x => new TestModel(x.Name)");
        TestModelSelector(x => new TestModel(x.Name) { Name = "nm" })
            .Returns("x => new TestModel(x.Name) { Name = \"nm\" }");
        TestModelSelector(x => new TestModel { Name = x.Name })
            .Returns("x => new TestModel { Name = x.Name }");
        TestModelSelector(x => new { })
            .Returns("x => new { }");
        TestModelSelector(x => new { x.Name })
            .Returns("x => new { Name = x.Name }");
        TestModelSelector(x => new { x.Name, Id = 0 })
            .Returns("x => new { Name = x.Name, Id = 0 }");

        // Array construction:
        TestModelSelector(x => new[] { new { x.Name }, new { Name = "nm" } })
            .Returns("x => [new { Name = x.Name }, new { Name = \"nm\" }]");
        TestModelSelector(x => new object[] { x.Name, 1 })
            .Returns("x => [x.Name, 1]");

        return items;
    }

    [TestCaseSource(nameof(GetExpressionTestCases))]
    public static string ExpressionToString(Expression expression) =>
        ImprovedExpressionStringBuilder.ExpressionToString(expression);

    public static class StaticClass
    {
        public static bool GetBool() => false;

        public static int GetInt() => 42;

        public static string GetString(string value) => value;
    }

    public abstract class TestComponent
    {
        public TestItem Item { get; private set; } = null!;

        public TestItem Item2 { get; private set; } = null!;

        public TestFlagValues Flags { get; private set; }

        public abstract bool IsIt(TestFlagValues flags);

        public abstract bool TryGetValue(string key, out int value);

        public abstract bool UseRefValue(string key, ref int value);
    }

    public class TestItem
    {
        public TestItemAttributes Attributes { get; private set; } = null!;

        public string Value { get; private set; } = string.Empty;

        public static implicit operator string?(TestItem? item) =>
            item?.ToString();
    }

    public abstract class TestItemAttributes
    {
        public bool Checked { get; private set; }

        public abstract string this[string index] { get; }

        public abstract TValue GetValue<TValue>(string attributeName);
    }

    public class TestModel
    {
        public TestModel()
            : this(null!)
        {
        }

        public TestModel(string name) =>
            Name = name;

        public string Name { get; set; }
    }
}
