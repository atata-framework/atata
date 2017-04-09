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
    }
}
