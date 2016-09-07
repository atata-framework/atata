using _ = Atata.Tests.TablePage;

namespace Atata.Tests
{
    [NavigateTo("Table.html")]
    [VerifyTitle]
    public class TablePage : Page<_>
    {
        public Table<_> SimpleTable { get; private set; }

        public Table<UserTableRow, _> ComplexTable { get; private set; }

        public Table<UserNavigatableTableRow, _> NavigatableTable { get; private set; }

        [FindByIndex(1)]
        public Table<CountryTableRow, _> CountryTable { get; private set; }

        [FindByIndex(1)]
        public Table<CountryByColumnIndexTableRow, _> CountryByColumnIndexTable { get; private set; }

        [FindByInnerXPath(".//th[.='Name'] and .//th[.='Description']")]
        public Table<EmptyTableRow, _> EmptyTable { get; private set; }

        public class UserTableRow : TableRow<_>
        {
            public Text<_> FirstName { get; private set; }

            public Text<_> LastName { get; private set; }
        }

        public class UserNavigatableTableRow : TableRow<_>, INavigable<GoTo1Page, _>
        {
            public Text<_> FirstName { get; private set; }

            public Text<_> LastName { get; private set; }
        }

        public class CountryTableRow : TableRow<_>
        {
            public Text<_> Country { get; private set; }

            public Text<_> Capital { get; private set; }
        }

        public class CountryByColumnIndexTableRow : TableRow<_>
        {
            [FindByColumnIndex(0)]
            public Text<_> CountryName { get; private set; }

            [FindByColumnIndex(1)]
            public Text<_> CapitalName { get; private set; }
        }

        public class EmptyTableRow : TableRow<_>
        {
            public Text<_> Name { get; private set; }

            public Text<_> Description { get; private set; }
        }
    }
}
