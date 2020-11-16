using _ = Atata.Tests.TablePage;

namespace Atata.Tests
{
    [Url("table")]
    [VerifyTitle]
    public class TablePage : Page<_>
    {
        public Table<_> SimpleTable { get; private set; }

        public Table<UserTableRow, _> ComplexTable { get; private set; }

        public Table<UserNavigatableTableRow, _> NavigatableTable { get; private set; }

        [Term("Add USA")]
        public Button<_> AddUsa { get; private set; }

        [Term(TermMatch.StartsWith)]
        public Button<_> AddChina { get; private set; }

        public Button<_> ClearChina { get; private set; }

        [FindByIndex(1)]
        public Table<CountryTableRow, _> CountryTable { get; private set; }

        [FindByIndex(1)]
        public Table<CountryByColumnIndexTableRow, _> CountryByColumnIndexTable { get; private set; }

        [FindById(CutEnding = false)]
        public Table<KeyValueItemTableRow, _> SingleItemTable { get; private set; }

        [FindById(CutEnding = false)]
        public Table<KeyValueItemTableRow, _> DuplicateItemsTable { get; private set; }

        [FindByInnerXPath(".//th[.='Name'] and .//th[.='Description']")]
        public Table<EmptyTableRow, _> EmptyTable { get; private set; }

        [FindById(CutEnding = false)]
        public Table<InsideAnotherTableRow, _> InsideAnotherTable { get; private set; }

        [FindById]
        public Table<CarTableRow, _> CarsTable { get; private set; }

        [FindById(CutEnding = false)]
        public Table<OrderedTableRow, _> OrderedTable { get; private set; }

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

        public class KeyValueItemTableRow : TableRow<_>
        {
            public Text<_> Key { get; private set; }

            public Text<_> Value { get; private set; }
        }

        public class EmptyTableRow : TableRow<_>
        {
            public Text<_> Name { get; private set; }

            public Text<_> Description { get; private set; }
        }

        public class InsideAnotherTableRow : TableRow<_>
        {
            public Text<_> Key { get; private set; }

            public Number<_> Value { get; private set; }
        }

        public class CarTableRow : TableRow<_>
        {
            public Text<_> CarMake { get; private set; }

            public Text<_> CarModel { get; private set; }

            public Content<int, _> Year { get; private set; }

            public Text<_> Category { get; private set; }

            public Text<_> AirConditioner { get; private set; }

            public DataProvider<bool?, _> HasAirConditioner => GetOrCreateDataProvider<bool?>(
                "has air conditioner",
                () => AirConditioner == "Yes");
        }

        public class OrderedTableRow : TableRow<_>
        {
            public Text<_> Letter { get; private set; }

            public Number<_> Number { get; private set; }
        }
    }
}
