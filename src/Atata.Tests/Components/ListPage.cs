using _ = Atata.Tests.ListPage;

namespace Atata.Tests
{
    [Url("List.html")]
    [VerifyTitle]
    [VerifyH1]
    public class ListPage : Page<_>
    {
        [ControlDefinition("li[parent::ul]/span[1]", ComponentTypeName = "product name")]
        public ControlList<Text<_>, _> ProductNameTextContolList { get; private set; }

        [ControlDefinition("li[parent::ul]/span[2]", ComponentTypeName = "product percent")]
        [Format("p")]
        public ControlList<Number<_>, _> ProductPercentNumberContolList { get; private set; }

        public UnorderedList<ListItem<_>, _> SimpleUnorderedList { get; private set; }

        public UnorderedList<UnorderedListItem, _> ComplexUnorderedList { get; private set; }

        public OrderedList<ListItem<_>, _> SimpleOrderedList { get; private set; }

        public OrderedList<OrderedListItem, _> ComplexOrderedList { get; private set; }

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
    }
}
