using _ = Atata.Tests.ClickPage;

namespace Atata.Tests
{
    [Url("actions/click")]
    [VerifyTitle]
    public class ClickPage : Page<_>
    {
        public Table<ClickableCellsTableRow, _> ClickableCellsTable { get; private set; }

        [FindById]
        public Content<int, _> DoubleClickBlock { get; set; }

        [FindById]
        public Content<int, _> RightClickBlock { get; set; }

        public class ClickableCellsTableRow : TableRow<_>
        {
            public ControlList<Content<int, _>, _> Cells { get; private set; }
        }
    }
}
