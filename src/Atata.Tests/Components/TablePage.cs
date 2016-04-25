using _ = Atata.Tests.TablePage;

namespace Atata.Tests
{
    [NavigateTo("http://localhost:50549/Table.html")]
    [VerifyTitle("Table")]
    public class TablePage : Page<_>
    {
        public Table<_> SimpleTable { get; private set; }

        public Table<UserTableRow, _> ComplexTable { get; private set; }

        public Table<UserNavigatableTableRow, _> NavigatableTable { get; private set; }

        public class UserTableRow : TableRow<_>
        {
            public Text<_> FirstName { get; private set; }

            public Text<_> LastName { get; private set; }
        }

        public class UserNavigatableTableRow : TableRow<GoTo1Page, _>
        {
            public Text<_> FirstName { get; private set; }

            public Text<_> LastName { get; private set; }
        }
    }
}
