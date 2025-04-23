namespace Atata.UnitTests.Expressions;

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

        TestCaseData TestIntOperation(Expression<Func<int, int>> expression) =>
            Test(expression);

        TestCaseData TestModelWithIndexPredicate(Expression<Func<TestModel, int, bool>> expression) =>
            Test(expression);

        TestCaseData TestModelSelector(Expression<Func<TestModel, object?>> expression) =>
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

        // Operators:
        TestIntOperation(x => x % 2)
            .Returns("x => x % 2");
        TestIntOperation(x => x ^ 2)
            .Returns("x => x ^ 2");
        TestIntOperation(x => x * -2)
            .Returns("x => x * -2");
        TestIntOperation(x => (int)(2L - x))
            .Returns("x => 2 - x");
        TestIntOperation(x => Interlocked.Increment(ref x))
            .Returns("x => Interlocked.Increment(ref x)");

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

        // Extension method:
        TestPredicate(x => x.Item.Value.Where(c => char.IsDigit(c)).Select(c => c.ToString()).Any())
            .Returns("x => x.Item.Value.Where(c => char.IsDigit(c)).Select(c => c.ToString()).Any()");

        // Instance method:
        TestPredicate(x => x.Item.Attributes.GetValue<DateTime>("data-date") <= DateTime.Today)
            .Returns("x => x.Item.Attributes.GetValue<DateTime>(\"data-date\") <= DateTime.Today");
        TestPredicate(x => x.Item.Attributes.GetValue<string, float>("key") <= 1.5)
            .Returns("x => x.Item.Attributes.GetValue<string, float>(\"key\") <= 1.5");

        // Instance method with out parameter:
        int outResult;

        TestPredicate(x => x.TryGetValue("key", out outResult))
            .Returns("x => x.TryGetValue(\"key\", out outResult)");

        // Instance method with ref parameter:
        int refValue = 1;

        TestPredicate(x => x.UseRefValue("key", ref refValue))
            .Returns("x => x.UseRefValue(\"key\", ref refValue)");

        // Static member:
        TestPredicate(x => x.Item.Value.Length == StaticClass.GetInt())
            .Returns("x => x.Item.Value.Length == ImprovedExpressionStringBuilderTests.StaticClass.GetInt()");
        TestPredicate(x => x.Item.Value.Contains(StaticClass.GetString(item.Name)))
            .Returns("x => x.Item.Value.Contains(ImprovedExpressionStringBuilderTests.StaticClass.GetString(item.Name))");
        TestPredicate(x => StaticClass.GetBool())
            .Returns("x => ImprovedExpressionStringBuilderTests.StaticClass.GetBool()");

#pragma warning disable CS0184 // 'is' expression's given expression is never of the provided type
        TestPredicate(x => new List<int?>().GetEnumerator() is List<int>.Enumerator)
            .Returns("x => new List<int?>().GetEnumerator() is List<int>.Enumerator");
#pragma warning restore CS0184 // 'is' expression's given expression is never of the provided type

        TestPredicate(x => StaticGenericClass<int>.GetValue() == 1)
            .Returns("x => ImprovedExpressionStringBuilderTests.StaticGenericClass<int>.GetValue() == 1");
        TestPredicate(x => StaticGenericClass<int>.Value > 1)
            .Returns("x => ImprovedExpressionStringBuilderTests.StaticGenericClass<int>.Value > 1");

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
            .Returns("x => new ImprovedExpressionStringBuilderTests.TestModel()");
        TestModelSelector(x => new TestModel(x.Name))
            .Returns("x => new ImprovedExpressionStringBuilderTests.TestModel(x.Name)");
        TestModelSelector(x => new TestModel(x.Name) { Name = "nm" })
            .Returns("x => new ImprovedExpressionStringBuilderTests.TestModel(x.Name) { Name = \"nm\" }");
        TestModelSelector(x => new TestModel { Name = x.Name })
            .Returns("x => new ImprovedExpressionStringBuilderTests.TestModel { Name = x.Name }");
        TestModelSelector(x => new { })
            .Returns("x => new { }");
        TestModelSelector(x => new { x.Name })
            .Returns("x => new { Name = x.Name }");
        TestModelSelector(x => new { x.Name, Id = 0 })
            .Returns("x => new { Name = x.Name, Id = 0 }");
        TestModelSelector(x => new Dictionary<int, string>())
            .Returns("x => new Dictionary<int, string>()");

        // Array construction:
        TestModelSelector(x => new[] { new { x.Name }, new { Name = "nm" } })
            .Returns("x => [new { Name = x.Name }, new { Name = \"nm\" }]");
        TestModelSelector(x => new object[] { x.Name, 1 })
            .Returns("x => [x.Name, 1]");

        // Other:
        TestModelSelector(x => (object)x as List<int>)
            .Returns("x => x as List<int>");
        TestModelSelector(x => default(int))
            .Returns("x => 0");
        TestModelSelector(x => default(List<int>))
            .Returns("x => null");
        TestModelSelector(x => typeof(List<int>))
            .Returns("x => typeof(List<int>)");

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

    public static class StaticGenericClass<T>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static T Value => default!;

        public static T GetValue() => default!;
#pragma warning restore CA1000 // Do not declare static members on generic types
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

        public abstract TValue GetValue<TKey, TValue>(TKey key);
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
