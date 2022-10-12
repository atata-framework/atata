namespace Atata.IntegrationTests.Controls;

public class ProviderNameTests : UITestFixture
{
    private TestPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<TestPage>(url: "list");

    [Test]
    public void OfStaticControlList()
    {
        var sut = _page.Folders;

        AssertProviderNameIs(sut, "folders");
    }

    [Test]
    public void OfDynamicControlList()
    {
        var sut = _page.FindAll<H1<TestPage>>();

        AssertProviderNameIs(sut, "<h1> heading items");
    }

    [Test]
    public void OfDynamicControlListWithName()
    {
        var sut = _page.FindAll<H1<TestPage>>("headings");

        AssertProviderNameIs(sut, "headings");
    }

    [Test]
    public void OfStaticControlListFieldItem_ByIndex()
    {
        var sut = _page.Files[0];

        AssertProviderNameIs(sut, "files / 1st item content");
    }

    [Test]
    public void OfStaticControlListWithNameAttributeFieldItem_ByIndex()
    {
        var sut = _page.FoldersAndFiles[1];

        AssertProviderNameIs(sut, "folders & files / 2nd item content");
    }

    [Test]
    public void OfDynamicControlListFieldItem_ByIndex()
    {
        var sut = _page.FindAll<H1<TestPage>>("headings")[0];

        AssertProviderNameIs(sut, "headings / 1st item content");
    }

    [Test]
    public void OfDynamicControlListFieldItem_ByPredicate()
    {
        var sut = _page.FindAll<Label<TestPage>>()[x => x != "none"];

        AssertProviderNameIs(sut, "label items / [x != \"none\"] item content");
    }

    [Test]
    public void OfDynamicControlListItemValueProvider_ByPredicate()
    {
        var sut = _page
            .FindAll<Label<TestPage>>("labels")[x => x != "a" && x != "b"]
            .DomProperties.Title;

        AssertProviderNameIs(sut, "labels / [x != \"a\" && x != \"b\"] item \"title\" DOM property");
    }

    [Test]
    public void OfControlList_InDynamicItemsControl()
    {
        var sut = _page
            .Find<ItemsControl<H1<TestPage>, TestPage>>("IC")
            .Items;

        AssertProviderNameIs(sut, "\"IC\" control / items");
    }

    [Test]
    public void OfControlList_InDynamicItemsControlItemThatIsInControlList()
    {
        var sut = _page
            .FindAll<UnorderedList<TestPage>>("<ul>'s")[0]
            .Items;

        AssertProviderNameIs(sut, "<ul>'s / 1st item / items");
    }

    [Test]
    public void OfDynamicControlListFieldItem_InItemsControlItemThatIsInControlList()
    {
        var sut = _page
            .FindAll<UnorderedList<TestPage>>("<ul>'s")[0]
            .Items[1];

        AssertProviderNameIs(sut, "<ul>'s / 1st item / items / 2nd item content");
    }

    [Test]
    public void OfStaticControlListFieldItem_InControlListItemThatIsInControlList()
    {
        var sut = _page
            .Folders[2]
            .Files[3];

        AssertProviderNameIs(sut, "folders / 3rd item / files / 4th item content");
    }

    [Test]
    public void OfStaticControlListFieldItemValueProvider_InControlListItemThatIsInControlList()
    {
        var sut = _page
            .Folders[2]
            .Files[3]
            .Name;

        AssertProviderNameIs(sut, "folders / 3rd item / files / 4th item / \"Name\" element content");
    }

    [Test]
    public void OfControlList_GetByXPathCondition()
    {
        var sut = _page
            .Folders[2]
            .Files.GetByXPathCondition("@id!='x'")
            .Name;

        AssertProviderNameIs(sut, "folders / 3rd item / files / [@id!='x'] item / \"Name\" element content");
    }

    [Test]
    public void OfControlList_GetByXPathCondition_WithItemName()
    {
        var sut = _page
            .Files.GetByXPathCondition("Id!=x", "@id!='x'");

        AssertProviderNameIs(sut, "files / \"Id!=x\" item content");
    }

    [Test]
    public void OfControlList_GetAllByXPathCondition()
    {
        var sut = _page
            .Folders[0]
            .Files.GetAllByXPathCondition("@id!='x'");

        AssertProviderNameIs(sut, "folders / 1st item / files / [@id!='x'] items");
    }

    [Test]
    public void OfControlList_GetAllByXPathCondition_First()
    {
        var sut = _page
            .Folders[0]
            .Files.GetAllByXPathCondition("@id!='x'")
            .First();

        AssertProviderNameIs(sut, "folders / 1st item / files / [@id!='x'] items / 1st item content");
    }

    [Test]
    public void OfControlList_GetAllByXPathCondition_WithItemsName()
    {
        var sut = _page
            .Folders[0]
            .Files.GetAllByXPathCondition("some", "@id!='x'");

        AssertProviderNameIs(sut, "folders / 1st item / files / \"some\" items");
    }

    [Test]
    public void OfControlList_GetAllByXPathCondition_WithItemsName_First()
    {
        var sut = _page
            .Folders[0]
            .Files.GetAllByXPathCondition("some", "@id!='x'")
            .First();

        AssertProviderNameIs(sut, "folders / 1st item / files / \"some\" items / 1st item content");
    }

    [Test]
    public void OfControlList_Contents()
    {
        var sut = _page
            .Folders[2]
            .Files.Contents
            .Skip(2)
            .First();

        AssertProviderNameIs(sut, "folders / 3rd item / files / contents.Skip(2).First()");
    }

    [Test]
    public void OfControlList_Count()
    {
        var sut = _page
            .Folders[2]
            .Files.Count;

        AssertProviderNameIs(sut, "folders / 3rd item / files count");
    }

    [Test]
    public void OfControlList_IndexOf()
    {
        var sut = _page
            .Folders[2]
            .Files.IndexOf(x => x.Name == "a");

        AssertProviderNameIs(sut, "folders / 3rd item / files / [Name == \"a\"] item index");
    }

    [Test]
    public void OfControlList_SelectData_WithSelecor()
    {
        var sut = _page
            .Folders[2]
            .Files.SelectData(x => x.Name.Value)
            .First();

        AssertProviderNameIs(sut, "folders / 3rd item / files / \"Name.Value\" values.First()");
    }

    [Test]
    public void OfControlList_SelectData_WithElementValueJSPath()
    {
        var sut = _page
            .Folders[2]
            .Files.SelectData<string>("getAttribute('data-id')")
            .First();

        AssertProviderNameIs(sut, "folders / 3rd item / files / JSPath \"getAttribute('data-id')\" values.First()");
    }

    [Test]
    public void OfControlList_SelectData_WithElementValueJSPathAndValueProviderName()
    {
        var sut = _page
            .Folders[2]
            .Files.SelectData<string>("getAttribute('data-id')", "ID")
            .First();

        AssertProviderNameIs(sut, "folders / 3rd item / files / \"ID\" values.First()");
    }

    [Test]
    public void OfControlList_SelectDataByExtraXPath()
    {
        var sut = _page
            .Folders[2]
            .Files.SelectDataByExtraXPath<string>("/h4", "getAttribute('data-id')")
            .First();

        AssertProviderNameIs(sut, "folders / 3rd item / files / XPath \"/h4\" elements / JSPath \"getAttribute('data-id')\" values.First()");
    }

    [Test]
    public void OfControlList_SelectDataByExtraXPath_WithValueProviderName()
    {
        var sut = _page
            .Folders[2]
            .Files.SelectDataByExtraXPath<string>("/h4", "getAttribute('data-id')", "ID")
            .First();

        AssertProviderNameIs(sut, "folders / 3rd item / files / \"ID\" values.First()");
    }

    [Test]
    public void OfControlList_SelectContentsByExtraXPath()
    {
        var sut = _page
            .Folders[2]
            .Files.SelectContentsByExtraXPath<int>("/h4")
            .First();

        AssertProviderNameIs(sut, "folders / 3rd item / files / XPath \"/h4\" elements contents.First()");
    }

    [Test]
    public void OfControlList_SelectContentsByExtraXPath_WithValueProviderName()
    {
        var sut = _page
            .Folders[2]
            .Files.SelectContentsByExtraXPath("/h4", "Header")
            .First();

        AssertProviderNameIs(sut, "folders / 3rd item / files / \"Header\" contents.First()");
    }

    [Test]
    public void OfControlList_Select_Count()
    {
        var sut = _page
            .Folders[2]
            .Files.Select(x => x.Name)
            .Count(x => x != "a");

        AssertProviderNameIs(sut, "folders / 3rd item / files.Select(x => x.Name).Count(x => x != \"a\")");
    }

    [Test]
    public void OfControlList_Select_Where()
    {
        var sut = _page
            .Folders[2]
            .Files.Select(x => x.Name)
            .Where(x => x != "a");

        AssertProviderNameIs(sut, "folders / 3rd item / files.Select(x => x.Name).Where(x => x != \"a\")");
    }

    [Test]
    public void OfControlList_Select_First()
    {
        var sut = _page
            .Folders[2]
            .Files.Select(x => x.Name)
            .First(x => x != "a");

        AssertProviderNameIs(sut, "folders / 3rd item / files / 1st item / \"Name\" element content");
    }

    [Test]
    public void OfControlList_Skip_First()
    {
        var sut = _page.Files
            .Skip(1)
            .First(x => x != "none");

        AssertProviderNameIs(sut, "files / 2nd item content");
    }

    [Test]
    public void OfControlList_Query()
    {
        var sut = _page
            .Folders[2]
            .Files.Query("not A", q => q.Where(x => x.Name != "a"));

        AssertProviderNameIs(sut, "folders / 3rd item / files.not A");
    }

    [Test]
    public void OfControlList_Query_First()
    {
        var sut = _page
            .Folders[2]
            .Files.Query("A", q => q.Where(x => x.Name != "a"))
            .First();

        AssertProviderNameIs(sut, "folders / 3rd item / files / 1st item content");
    }

    private static void AssertProviderNameIs<TObject>(IObjectProvider<TObject> provider, string expected) =>
        provider.ProviderName.Should().Be(expected);

    public class TestPage : Page<TestPage>
    {
        public ControlList<FolderItem, TestPage> Folders { get; private set; }

        public ControlList<FileItem, TestPage> Files { get; private set; }

        [Name("folders & files")]
        public ControlList<Text<TestPage>, TestPage> FoldersAndFiles { get; private set; }

        [ControlDefinition("div")]
        public class FolderItem : Control<TestPage>
        {
            public ControlList<FileItem, TestPage> Files { get; private set; }
        }

        [ControlDefinition(ComponentTypeName = "file")]
        public class FileItem : Text<TestPage>
        {
            public Text<TestPage> Name { get; private set; }
        }
    }
}
