namespace Atata.IntegrationTests;

using _ = ListPage;

[Url("list")]
[VerifyTitle]
[VerifyH1]
public class ListPage : Page<_>
{
    [ControlDefinition("li[parent::ul]/span[1]", ComponentTypeName = "product name")]
    [FindSettings(OuterXPath = ".//div[@id='simple-list-section']/div[1]//")]
    public ControlList<Text<_>, _> ProductNameTextContolList { get; private set; }

    [ControlDefinition("li[parent::ul]/span[2]", ComponentTypeName = "product percent")]
    [FindSettings(OuterXPath = ".//div[@id='simple-list-section']//")]
    [Format("p")]
    public ControlList<Number<_>, _> ProductPercentNumberContolList { get; private set; }

    public UnorderedList<_> SimpleUnorderedList { get; private set; }

    public UnorderedList<UnorderedListItem, _> ComplexUnorderedList { get; private set; }

    public OrderedList<_> SimpleOrderedList { get; private set; }

    public OrderedList<OrderedListItem, _> ComplexOrderedList { get; private set; }

    [FindById("hierarchical-unordered-list")]
    public UnorderedList<ListItem<_>, _> UnorderedListForHierarchy { get; private set; }

    public HierarchicalUnorderedList<HierarchicalListItem<_>, _> PlainHierarchicalUnorderedList { get; private set; }

    [FindById("hierarchical-unordered-list")]
    public HierarchicalUnorderedList<HierarchicalListItem<_>, _> SimpleHierarchicalUnorderedList { get; private set; }

    [FindById("hierarchical-unordered-list")]
    public HierarchicalUnorderedList<HierarchicalUnorderedListItem, _> ComplexHierarchicalUnorderedList { get; private set; }

    [FindById("hierarchical-ordered-list")]
    public HierarchicalOrderedList<HierarchicalOrderedListItem, _> ComplexHierarchicalOrderedList { get; private set; }

    [FindById("hierarchical-ordered-list")]
    public HierarchicalOrderedList<HierarchicalOrderedListItemWithAnyVisibilityUsingControlDefinition, _> ComplexHierarchicalOrderedListWithAnyVisibilityUsingControlDefinition { get; private set; }

    [FindById("ul-of-li-span")]
    public ItemsControl<Control<_>, _> ItemsControlOfDescendantsAsControls { get; private set; }

    [FindById("ul-of-li-span")]
    [FindSettings(OuterXPath = "./", TargetName = "Items")]
    public ItemsControl<Control<_>, _> ItemsControlOfChildrenAsControls { get; private set; }

    public class UnorderedListItem : ListItem<_>
    {
        [FindByIndex(0)]
        public Text<_> Name { get; private set; }

        [FindByIndex(1)]
        [Format("p")]
        public Number<_> Percent { get; private set; }
    }

    public class OrderedListItem : ListItem<_>
    {
        [FindByXPath("span[1]")]
        public Text<_> Name { get; private set; }

        [FindByXPath("span[2]")]
        public Number<_> Amount { get; private set; }
    }

    public class HierarchicalUnorderedListItem : HierarchicalListItem<HierarchicalUnorderedListItem, _>
    {
        [FindByXPath("./span[1]")]
        public Text<_> Name { get; private set; }
    }

    [FindOnlyVisible]
    public class HierarchicalOrderedListItem : HierarchicalListItem<HierarchicalOrderedListItem, _>
    {
        [FindByXPath("./span[1]")]
        public Text<_> Name { get; private set; }

        [FindByClass]
        public Number<_> Number { get; private set; }
    }

    [ControlDefinition("li", ComponentTypeName = "list item", Visibility = Visibility.Any)]
    [GetsContentFromSource(ContentSource.TextContent, TargetSelfAndChildren = true)]
    public class HierarchicalOrderedListItemWithAnyVisibilityUsingControlDefinition : HierarchicalListItem<HierarchicalOrderedListItemWithAnyVisibilityUsingControlDefinition, _>
    {
        [FindByXPath("./span[1]")]
        public Text<_> Name { get; private set; }

        [FindByClass]
        public Number<_> Number { get; private set; }
    }
}
